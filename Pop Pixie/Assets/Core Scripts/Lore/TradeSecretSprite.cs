using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSecretSprite : AInspectable, ISerializableComponent {

  public string[] SerializableFields { get; } = { "Collected" };

  public bool Collected = false;
  public LoreItem LoreItem;
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
    StateManager.SetState(State.Lore);

    LoreManager.Current.Open(LoreItem, () => {
      StateManager.SetState(State.Playing);
    });
  }

  public override String AInspectablePromptText() {
    return "Press <color=#00ffff>[Interact]</color> to steal <color=#ffff00>Trade Secret</color>";
  }
}
