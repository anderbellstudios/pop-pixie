using System.Collections.Generic;
using System.Linq;
using MidiParser;
using TMPro;
using UnityEngine;

public enum RhythmGameNoteType {
  Left = 0,
  Down = 1,
  Up = 2,
  Right = 3,
};

[System.Serializable]
public class RhythmGameNote {
  public RhythmGameNoteType Type;
  public float Time;
  public float Duration = 0f;

  public bool IsHold => Duration > 0f;
  public bool IsInstant => !IsHold;

  public float RelativeTime(float currentTime, bool end = false) =>
    Time - currentTime + (end ? Duration : 0f);
}

[System.Serializable]
public class RhythmGameSong {
  public float BeatsPerMinute;
  public List<RhythmGameNote> Notes;

  private static readonly Dictionary<int, RhythmGameNoteType> PITCH_TO_NOTE_TYPE = new()
  {
    { 65, RhythmGameNoteType.Left },
    { 69, RhythmGameNoteType.Down },
    { 72, RhythmGameNoteType.Up },
    { 76, RhythmGameNoteType.Right },
  };

  public static RhythmGameSong ParseMidi(string path) {
    MidiFile midi = new MidiFile(path);

    if (midi.Tracks.Length != 1)
      throw new System.Exception("MIDI file should have exactly one track");

    MidiTrack track = midi.Tracks[0];

    List<RhythmGameNote> notes = new();
    float bpm = 120f;

    foreach (MidiEvent midiEvent in track.MidiEvents) {
      switch (midiEvent.MidiEventType) {
        case MidiEventType.MetaEvent:
          if (midiEvent.MetaEventType == MetaEventType.Tempo) {
            // TODO: Track a list of tempo change events on the song object
            bpm = (int)midiEvent.Arg2;
          }
          break;

        case MidiEventType.NoteOn:
          RhythmGameNoteType type = PITCH_TO_NOTE_TYPE[midiEvent.Note];
          float beat = (float)midiEvent.Time / midi.TicksPerQuarterNote;
          float time = beat * 60f / bpm;
          notes.Add(new() { Type = type, Time = time });
          break;

        // TODO: Parse hold notes
      }
    }

    return new RhythmGameSong { BeatsPerMinute = bpm, Notes = notes };
  }
}

public class RhythmGame : MonoBehaviour {
  public RhythmGameNotesView NotesView;
  public TMP_Text ScoreText;
  public TMP_Text NoteGradeText;

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

  void Start() {
    RhythmGameSong song = RhythmGameSong.ParseMidi(
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
  }

  private void HandleButtonUp(RhythmGameNoteType noteType) {
    if (HeldNotes.ContainsKey(noteType)) {
      ReleaseNote(HeldNotes[noteType]);
    }
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
    if (closeEnough)
      remainingDuration = 0f;

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
  }

  private void NoteNotFound(RhythmGameNoteType noteType) {
    UpdateScore(MISS_NOTE_SCORE);
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

  private float CurrentTime => Time.time;

  private List<RhythmGameNote> PressableNotes => PressableNotesWindow.Current;
}
