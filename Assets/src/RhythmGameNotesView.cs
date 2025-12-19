using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameNotesView : MonoBehaviour {
  public float PixelsBetweenEachBeat;
  public Transform LeftTarget;
  public Transform DownTarget;
  public Transform UpTarget;
  public Transform RightTarget;
  public GameObject ModelNote;

  private LinearWindow<RhythmGameNote> VisibleNotesWindow;
  private Func<float> GetTime;
  private Dictionary<RhythmGameNote, GameObject> NoteGameObjects = new();

  private bool AfterFirstUpdate = false;
  private float TargetY;
  private float SpawnY;
  private float DespawnY;
  private float NoteSpeed;

  public void Init(RhythmGameSong song, Func<float> getTime) {
    VisibleNotesWindow = new(song.Notes, onEnterWindow: SpawnNote, onExitWindow: DespawnNote);
    GetTime = getTime;

    TargetY = transform.InverseTransformPoint(LeftTarget.position).y;

    float noteSize = ModelNote.GetComponent<RectTransform>().rect.height;
    float bottomOfScreen = GetComponent<RectTransform>().rect.yMin;
    float topOfScreen = GetComponent<RectTransform>().rect.yMax;
    SpawnY = bottomOfScreen - noteSize / 2f;
    DespawnY = topOfScreen + noteSize / 2f;

    float secondsPerBeat = 60f / song.BeatsPerMinute;
    NoteSpeed = PixelsBetweenEachBeat / secondsPerBeat;
  }

  void Update() {
    /**
     * Do not spawn any notes during the first update, since the positions can
     * be incorrect.
     */
    if (!AfterFirstUpdate) {
      AfterFirstUpdate = true;
      return;
    }

    VisibleNotesWindow.Update(note => {
      float y = YPositionForNote(note);
      return y >= SpawnY && y <= DespawnY;
    });

    foreach (RhythmGameNote note in VisibleNotes) {
      if (NoteGameObjects.ContainsKey(note)) {
        float y = YPositionForNote(note);
        Transform noteTransform = NoteGameObjects[note].transform;
        noteTransform.localPosition = new Vector2(noteTransform.localPosition.x, y);
      }
    }
  }

  public void HitNote(RhythmGameNote note) {
    DespawnNote(note);
    VisibleNotesWindow.RemoveEarly(note);
  }

  private float YPositionForNote(RhythmGameNote note) {
    float distanceToTarget = note.RelativeTime(CurrentTime) * NoteSpeed;
    return TargetY - distanceToTarget;
  }

  private void SpawnNote(RhythmGameNote note) {
    GameObject noteGameObject = Instantiate(ModelNote, transform);
    noteGameObject.SetActive(true);
    NoteGameObjects.Add(note, noteGameObject);

    Transform target = TargetForNoteType(note.Type);
    float spawnX = transform.InverseTransformPoint(target.position).x;

    noteGameObject.transform.localPosition = new Vector2(spawnX, SpawnY);
    noteGameObject.transform.localRotation = target.localRotation;
    noteGameObject.GetComponent<Image>().color = target.GetComponent<Image>().color;
  }

  private void DespawnNote(RhythmGameNote note) {
    if (NoteGameObjects.ContainsKey(note)) {
      Destroy(NoteGameObjects[note]);
      NoteGameObjects.Remove(note);
    }
  }

  private Transform TargetForNoteType(RhythmGameNoteType noteType) {
    switch (noteType) {
      case RhythmGameNoteType.Left:
        return LeftTarget;

      case RhythmGameNoteType.Down:
        return DownTarget;

      case RhythmGameNoteType.Up:
        return UpTarget;

      case RhythmGameNoteType.Right:
        return RightTarget;
    }

    throw new System.ArgumentException("Unexpected note type");
  }

  public List<RhythmGameNote> VisibleNotes => VisibleNotesWindow.Current;
  private float CurrentTime => GetTime();
}
