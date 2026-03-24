using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RhythmGame : MonoBehaviour {
  public FMODUnity.StudioEventEmitter MusicEventEmitter;
  public FMODUnity.StudioEventEmitter MistakeSnapshot;
  public PlaySound MistakeSound;
  public RhythmGameNotesView NotesView;
  public TMP_Text ScoreText;
  public TMP_Text NoteGradeText;
  public float MistakeSoundDebounce;
  public float MistakeSnapshotDuration;

  /**
   * Constants and scoring algorithm modified from the source code of Friday
   * Night Funkin' and used under the following license:
   * https://github.com/FunkinCrew/Funkin/blob/24250549406207988b2718f5a05bf31e5af7629e/LICENSE.md
   */
  private const float MISS_THRESHOLD = 0.16f;
  private const float DROP_THRESHOLD = 0.08f;
  private const int MAX_NOTE_SCORE = 500;
  private const int MIN_NOTE_SCORE = 9;
  private const int MISS_NOTE_SCORE = -100;
  private const float SCORING_SLOPE = 80f;
  private const float SCORING_OFFSET = 0.05499f;
  private const float HOLD_SCORE_PER_SECOND = 250f;
  private const float DROP_SCORE_PER_SECOND = -125f;

  private readonly (float, string)[] NOTE_GRADES = {
    (0.012f, "Poppin'"),
    (0.045f, "Perfect"),
    (0.09f, "Pretty good"),
    (0.135f, "Poor"),
    (MISS_THRESHOLD, "Pathetic"),
  };

  private LinearWindow<RhythmGameNote> PressableNotesWindow;
  private Dictionary<RhythmGameNoteType, RhythmGameNote> HeldNotes = new();
  private float Score = 0f;
  private int MaxPossibleScore;
  private int FMODPreviousTimelinePositionMS = -1;
  private Stopwatch FMODTimelineLastUpdateStopwatch;
  private Stopwatch MistakeSoundDebounceStopwatch;
  private AsyncTimer.EnqueuedEvent ClearMistakeSnapshot;

  void Start() {
    RhythmGameSong song = RhythmGameSongParser.ParseMidi(
      System.IO.Path.Combine(Application.streamingAssetsPath, "Rhythm Game Data", "Demo.mid")
    );

    PressableNotesWindow = new(song.Notes, onExitWindow: MissedNote);
    NotesView.Init(song, getTime: () => CurrentTime);

    MaxPossibleScore = song
      .Notes.Select(note => {
        int perfectHitScore = ScoreNoteHit(0f);
        int holdScore = (int)(note.Duration * HOLD_SCORE_PER_SECOND);
        return perfectHitScore + holdScore;
      })
      .Sum();

    Debug.Log("Max possible score: " + MaxPossibleScore.ToString());

    UpdateScore(0f);
  }

  void Update() {
    PressableNotesWindow.Update(note =>
      note.RelativeTime(CurrentTime) <= MISS_THRESHOLD
      && note.RelativeTime(CurrentTime, end: true) >= -MISS_THRESHOLD
    );

    if (WrappedInput.GetButtonDown("Rhythm Game Left")) {
      HandleButtonDown(RhythmGameNoteType.Left);
    }

    if (WrappedInput.GetButtonDown("Rhythm Game Down")) {
      HandleButtonDown(RhythmGameNoteType.Down);
    }

    if (WrappedInput.GetButtonDown("Rhythm Game Up")) {
      HandleButtonDown(RhythmGameNoteType.Up);
    }

    if (WrappedInput.GetButtonDown("Rhythm Game Right")) {
      HandleButtonDown(RhythmGameNoteType.Right);
    }

    if (WrappedInput.GetButtonUp("Rhythm Game Left")) {
      HandleButtonUp(RhythmGameNoteType.Left);
    }
    if (WrappedInput.GetButtonUp("Rhythm Game Down")) {
      HandleButtonUp(RhythmGameNoteType.Down);
    }

    if (WrappedInput.GetButtonUp("Rhythm Game Up")) {
      HandleButtonUp(RhythmGameNoteType.Up);
    }

    if (WrappedInput.GetButtonUp("Rhythm Game Right")) {
      HandleButtonUp(RhythmGameNoteType.Right);
    }
  }

  private void HandleButtonDown(RhythmGameNoteType noteType) {
    /**
     * If there are multiple matching notes, return the earliest one so that
     * notes are pressed in the correct order.
     */
    RhythmGameNote note = PressableNotes.Find(note => note.Type == noteType);

    if (note == null) {
      NoteNotFound(noteType);
    } else {
      HitNote(note);
    }

    NotesView.HandleButtonDown(noteType);
  }

  private void HandleButtonUp(RhythmGameNoteType noteType) {
    if (HeldNotes.ContainsKey(noteType)) {
      ReleaseNote(HeldNotes[noteType]);
    }

    NotesView.HandleButtonUp(noteType);
  }

  private void HitNote(RhythmGameNote note) {
    PressableNotesWindow.RemoveEarly(note);
    NotesView.HitNote(note);

    if (note.IsHold) {
      HeldNotes.Add(note.Type, note);
    }

    float relativeTime = Mathf.Abs(note.RelativeTime(CurrentTime));
    UpdateScore(ScoreNoteHit(relativeTime));
    ShowNoteGrade(GradeNoteHit(relativeTime));
  }

  private void ReleaseNote(RhythmGameNote note) {
    float remainingDuration = note.RelativeTime(CurrentTime, end: true);

    bool closeEnough = remainingDuration <= DROP_THRESHOLD;
    if (closeEnough) {
      remainingDuration = 0f;
    } else {
      PlayMistakeSound();
    }

    float heldDuration = note.Duration - remainingDuration;

    HeldNotes.Remove(note.Type);
    NotesView.ReleaseNote(note, closeEnough: closeEnough);

    float holdScore = heldDuration * HOLD_SCORE_PER_SECOND;
    float dropScore = remainingDuration * DROP_SCORE_PER_SECOND;
    int scoreChange = (int)(holdScore + dropScore);
    UpdateScore(scoreChange);
  }

  private void MissedNote(RhythmGameNote note) {
    ShowNoteGrade("Miss");
    UpdateScore(MISS_NOTE_SCORE);
    PlayMistakeSound();
  }

  private void NoteNotFound(RhythmGameNoteType noteType) {
    UpdateScore(MISS_NOTE_SCORE);
    PlayMistakeSound();
  }

  private void PlayMistakeSound() {
    if (
      MistakeSoundDebounceStopwatch == null
      || MistakeSoundDebounceStopwatch.Time() >= MistakeSoundDebounce
    ) {
      MistakeSound.Play();
      MistakeSoundDebounceStopwatch = new Stopwatch.BaseTime();
    }

    if (ClearMistakeSnapshot == null) {
      MistakeSnapshot.Play();
    } else {
      AsyncTimer.BaseTime.ClearTimeout(ClearMistakeSnapshot);
    }

    ClearMistakeSnapshot = AsyncTimer.BaseTime.SetTimeout(
      () => {
        MistakeSnapshot.Stop();
        ClearMistakeSnapshot = null;
      },
      MistakeSnapshotDuration
    );
  }

  private int ScoreNoteHit(float relativeTime) {
    /**
     * Although most misses are handled by MissedNote, this case can arise if a
     * hold note is pressed late.
     */
    if (relativeTime > MISS_THRESHOLD)
      return MISS_NOTE_SCORE;

    /**
     * Curve that outputs a value close to 1 at relativeTime = 0 and approaches
     * 0 at approximately relativeTime = 0.15.
     */
    float factor = 1f - 1f / (1f + Mathf.Exp(-SCORING_SLOPE * (relativeTime - SCORING_OFFSET)));

    return (int)(MAX_NOTE_SCORE * factor + MIN_NOTE_SCORE);
  }

  private string GradeNoteHit(float relativeTime) {
    foreach (var (threshold, label) in NOTE_GRADES) {
      if (relativeTime <= threshold)
        return label;
    }

    return "Miss";
  }

  private void UpdateScore(float scoreChange) {
    Score = Mathf.Max(0f, Score + scoreChange);
    int maxScoreDigits = Mathf.CeilToInt(Mathf.Log10((float)MaxPossibleScore));
    ScoreText.text = Score.ToString().PadLeft(maxScoreDigits, '0');
  }

  private void ShowNoteGrade(string grade) {
    NoteGradeText.text = grade;
  }

  /**
   * The timeline position reported by FMOD updates infrequently, so we smooth
   * it by tracking the time since it last changed.
   */
  private float CurrentTime {
    get {
      int timelinePositionMS = FMODTimelinePositionMS;

      if (timelinePositionMS == FMODPreviousTimelinePositionMS) {
        return FMODPreviousTimelinePositionMS / 1000f + FMODTimelineLastUpdateStopwatch.Time();
      }

      FMODPreviousTimelinePositionMS = timelinePositionMS;
      FMODTimelineLastUpdateStopwatch = new Stopwatch.BaseTime();
      return timelinePositionMS / 1000f;
    }
  }

  private int FMODTimelinePositionMS {
    get {
      MusicEventEmitter.EventInstance.getTimelinePosition(out int timelinePosition);
      return timelinePosition;
    }
  }

  private List<RhythmGameNote> PressableNotes => PressableNotesWindow.Current;
}
