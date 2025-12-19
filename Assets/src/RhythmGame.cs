using System.Collections.Generic;
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
  public int BeatsPerMinute;
  public List<RhythmGameNote> Notes;
}

public class RhythmGame : MonoBehaviour {
  public RhythmGameNotesView NotesView;
  public RhythmGameSong Song;

  public const float MISS_THRESHOLD = 0.16f;

  private LinearWindow<RhythmGameNote> PressableNotesWindow;
  private Dictionary<RhythmGameNoteType, RhythmGameNote> HeldNotes = new();

  void Start() {
    PressableNotesWindow = new(Song.Notes, onExitWindow: MissedNote);
    NotesView.Init(Song, getTime: () => CurrentTime);
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
      MissedNote(noteType);
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

      if (note.RelativeTime(CurrentTime) < -MISS_THRESHOLD) {
        Debug.Log("Miss (hold note started too late");
      }
    }
  }

  private void ReleaseNote(RhythmGameNote note) {
    float remainingDuration = note.RelativeTime(CurrentTime, end: true);
    bool closeEnough = remainingDuration <= MISS_THRESHOLD;

    HeldNotes.Remove(note.Type);
    NotesView.ReleaseNote(note, closeEnough: closeEnough);

    if (!closeEnough) {
      Debug.Log("Released note too early");
    }
  }

  private void MissedNote(RhythmGameNote note) {
    Debug.Log("Miss (note not pressed)");
  }

  private void MissedNote(RhythmGameNoteType noteType) {
    Debug.Log("Miss (no note found)");
  }

  private float CurrentTime => Time.time;

  private List<RhythmGameNote> PressableNotes => PressableNotesWindow.Current;
}
