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

  public float RelativeTime(float currentTime) => Time - currentTime;

  public float RelativeTimeAbs(float currentTime) => Mathf.Abs(Time - currentTime);
}

[System.Serializable]
public class RhythmGameSong {
  public int BeatsPerMinute;
  public List<RhythmGameNote> Notes;
}

public class RhythmGame : MonoBehaviour {
  public RhythmGameNotesView NotesView;
  public RhythmGameSong Song;
  public float MissThreshold;

  private LinearWindow<RhythmGameNote> PressableNotesWindow;

  void Start() {
    PressableNotesWindow = new(Song.Notes, onExitWindow: MissedNote);
    NotesView.Init(Song, getTime: () => CurrentTime);
  }

  void Update() {
    PressableNotesWindow.Update(note => note.RelativeTimeAbs(CurrentTime) <= MissThreshold);

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

  private void HitNote(RhythmGameNote note) {
    PressableNotesWindow.RemoveEarly(note);
    NotesView.HitNote(note);
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
