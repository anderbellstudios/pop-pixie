using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AInspectable : MonoBehaviour {

  public static Dictionary<AInspectable, bool> IsNearby = new Dictionary<AInspectable, bool>() {};

  public static bool ShowButtonPrompt() {
    return IsNearby.Values.Any( isTrue => isTrue );
  }

  bool _Nearby;
  bool _ButttonReleased;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      _Nearby = true;
      OnPlayerOver();
    }
  }

  void OnTriggerExit2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      _Nearby = false;
      OnPlayerOut();
    }
  }

  void Start() {
    AInspectableStart();
  }

  public void AInspectableStart() {
    InGamePrompt.Current.RegisterSource(() => {
      if (InspectImminent()) {
        return AInspectablePromptText();
      } else {
        return null;
      }
    });
  }

  public virtual String AInspectablePromptText() {
    return null;
  }

  void Update() {
    AInspectableUpdate();
  }

  public void AInspectableUpdate() {
    IsNearby[this] = InspectImminent();

    if (InspectImminent() && _ButttonReleased && WrappedInput.GetButtonDown("Inspect"))
      OnInspect();

    _ButttonReleased = StateManager.Playing && !WrappedInput.GetButton("Inspect");
  }

  bool InspectImminent() {
    return StateManager.Playing && _Nearby && IsInspectable();
  }

  public abstract void OnInspect();

  public virtual void OnPlayerOver() {
  }

  public virtual void OnPlayerOut() {
  }

  public virtual bool IsInspectable() {
    return true;
  }

}
