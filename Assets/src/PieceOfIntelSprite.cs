using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOfIntelSprite : AInspectable, ISerializableComponent {

  public string[] SerializableFields { get; } = { "Collected" };

  public bool Collected = false;
  public LoreItem LoreItem;
  public CaptionLine CaptionLineAfterClose;
  public GameObject SpriteGameObject;
  public CircleCollider2D ColliderBehaviour;

  void Update() {
    AInspectableUpdate();

    SpriteGameObject.SetActive(!Collected);
    ColliderBehaviour.enabled = !Collected;
  }

  public override void OnInspect() {
    Collected = true;

    LoreItemData.RecordRead(LoreItem);
    StateManager.AddState(State.NotPlaying);

    LoreManager.Current.Open(LoreItem, () => {
      StateManager.RemoveState(State.NotPlaying);
      CaptionLineManager.Current.Play(CaptionLineAfterClose);
    });
  }

  public override String AInspectablePromptText() {
    return "Press [Inspect] to steal <color=#ffff00>Piece of Intel</color>";
  }
}
