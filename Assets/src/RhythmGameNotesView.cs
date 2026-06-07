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
  public RhythmGameNoteSprite NoteSpriteTemplate;

  private LinearWindow<RhythmGameNote> VisibleNotesWindow;
  private Func<float> GetTime;
  private Dictionary<RhythmGameNote, RhythmGameNoteSprite> NoteSprites = new();

  private bool AfterFirstUpdate = false;
  private float TargetY;
  private float SpawnY;
  private float DespawnY;
  private float NoteSpeed;

  public void Init(RhythmGameSong song, Func<float> getTime) {
    VisibleNotesWindow = new(song.Notes, onEnterWindow: SpawnNote, onExitWindow: DespawnNote);
    GetTime = getTime;

    TargetY = transform.InverseTransformPoint(LeftTarget.position).y;

    float noteSize = NoteSpriteTemplate.ArrowHeight;
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

    VisibleNotesWindow.Update(note =>
      YPositionForNote(note) >= SpawnY && YPositionForNote(note, end: true) <= DespawnY
    );

    foreach (RhythmGameNote note in VisibleNotes) {
      if (NoteSprites.ContainsKey(note)) {
        RhythmGameNoteSprite noteSprite = NoteSprites[note];

        // Update note position
        float y = YPositionForNote(note);
        Transform noteTransform = noteSprite.transform;
        noteTransform.localPosition = new Vector2(noteTransform.localPosition.x, y);

        if (noteSprite.IsHolding) {
          noteSprite.UpdateHolding(CurrentTime);
        }
      }
    }
  }

  public void HandleButtonDown(RhythmGameNoteType noteType) {
    FlashForNoteType(noteType).SetOpacity(1f);
  }

  public void HandleButtonUp(RhythmGameNoteType noteType) {
    FlashForNoteType(noteType).Fade(0f);
  }

  public void HitNote(RhythmGameNote note) {
    if (note.IsInstant) {
      DespawnNote(note);
      VisibleNotesWindow.RemoveEarly(note);
    } else if (NoteSprites.ContainsKey(note)) {
      NoteSprites[note].StartHolding();
    }
  }

  public void ReleaseNote(RhythmGameNote note, bool closeEnough) {
    if (NoteSprites.ContainsKey(note)) {
      NoteSprites[note].StopHolding(CurrentTime, closeEnough: closeEnough);
    }
  }

  private float YPositionForNote(RhythmGameNote note, bool end = false) {
    float distanceToTarget = note.RelativeTime(CurrentTime, end: end) * NoteSpeed;
    return TargetY - distanceToTarget;
  }

  private void SpawnNote(RhythmGameNote note) {
    Transform target = TargetForNoteType(note.Type);
    float spawnX = transform.InverseTransformPoint(target.position).x;

    GameObject noteSpriteGameObject = Instantiate(NoteSpriteTemplate.gameObject, transform);
    RhythmGameNoteSprite noteSprite = noteSpriteGameObject.GetComponent<RhythmGameNoteSprite>();
    noteSprite.transform.localPosition = new Vector2(spawnX, SpawnY);
    NoteSprites.Add(note, noteSprite);

    noteSprite.Initialize(
      note: note,
      color: target.Find("Arrow").GetComponent<Image>().color,
      rotation: target.localRotation,
      trailLength: note.Duration * NoteSpeed
    );
  }

  private void DespawnNote(RhythmGameNote note) {
    if (NoteSprites.ContainsKey(note)) {
      Destroy(NoteSprites[note].gameObject);
      NoteSprites.Remove(note);
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

  private CanvasFadeInOut FlashForNoteType(RhythmGameNoteType noteType) =>
    TargetForNoteType(noteType).GetComponentInChildren<CanvasFadeInOut>();

  public List<RhythmGameNote> VisibleNotes => VisibleNotesWindow.Current;
  private float CurrentTime => GetTime();
}
