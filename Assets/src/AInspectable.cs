using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AInspectable : MonoBehaviour {

  public static Dictionary<AInspectable, bool> IsNearby = new Dictionary<AInspectable, bool>() { };

  public static bool ShowButtonPrompt() {
    return IsNearby.Values.Any(isTrue => isTrue);
  }

  protected bool _Nearby;
  ButtonPressHelper _ButtonPressHelper = new SingleButtonPressHelper();

  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      _Nearby = true;
      OnPlayerOver();
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.tag == "Player") {
      _Nearby = false;
      OnPlayerOut();
    }
  }

  void Start() {
    AInspectableStart();
  }

  public void AInspectableStart() {
    InGamePrompt.Current.RegisterSource(1000, () => {
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
    if (!StateManager.Playing)
      _ButtonPressHelper.Clear();

    IsNearby[this] = InspectImminent();

    if (InspectImminent() && _ButtonPressHelper.GetButtonPress("Inspect"))
      OnInspect();
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
