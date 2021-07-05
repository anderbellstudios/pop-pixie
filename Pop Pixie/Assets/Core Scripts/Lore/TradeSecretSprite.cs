using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSecretSprite : AInspectable, ISerializableComponent {

  public string[] SerializableFields { get; } = { "Collected" };

  public bool Collected = false;
  public GameObject SpriteGameObject;
  public CircleCollider2D ColliderBehaviour;

  void Update() {
    AInspectableUpdate();

    SpriteGameObject.SetActive(!Collected);
    ColliderBehaviour.enabled = !Collected;
  }

  public override void OnInspect() {
    Debug.Log("Activated");
    Collected = true;
  }
}
