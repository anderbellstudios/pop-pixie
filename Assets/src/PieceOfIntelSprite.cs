using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOfIntelSprite : AInspectable {
  public bool Collected = false;
  public LoreItem LoreItem;
  public CaptionLine CaptionLineAfterClose;
  public GameObject SpriteGameObject;
  public CircleCollider2D ColliderBehaviour;

  void Start() {
    PreloadProgrammerSounds.PreloadCaptionLine(CaptionLineAfterClose);
    if (LoreItemData.AlreadyRead(LoreItem)) {
      WasCollected();
    }
    AInspectableStart();
  }

  public override void OnInspect() {
    WasCollected();
    LoreItemData.RecordRead(LoreItem);

    StateManager.AddState(State.NotPlaying);

    LoreManager.Current.Open(LoreItem, () => {
      StateManager.RemoveState(State.NotPlaying);
      CaptionLineManager.Current.Play(CaptionLineAfterClose);
    });
  }

  private void WasCollected() {
    Collected = true;
    SpriteGameObject.SetActive(false);
    ColliderBehaviour.enabled = false;
  }

  public override String AInspectablePromptText() {
    return "Press [Inspect] to steal a <color=#ffff00>Piece of Intel</color>";
  }
}
