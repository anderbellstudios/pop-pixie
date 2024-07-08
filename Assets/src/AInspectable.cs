using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AInspectable : MonoBehaviour {
  public static Dictionary<AInspectable, bool> IsInspectImminentMap = new Dictionary<AInspectable, bool>() { };

  public static bool ShowButtonPrompt() {
    return IsInspectImminentMap.Values.Any(isTrue => isTrue);
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
      if (IsInspectImminent()) {
        return AInspectablePromptText();
      }

      if (IsPlayingAndNearby()) {
        return AInspectableUninspectableText();
      }

      return null;
    });
  }

  public virtual String AInspectablePromptText() {
    return null;
  }

  public virtual String AInspectableUninspectableText() {
    return null;
  }

  void Update() {
    AInspectableUpdate();
  }

  public void AInspectableUpdate() {
    if (!StateManager.Playing)
      _ButtonPressHelper.Clear();

    IsInspectImminentMap[this] = IsInspectImminent();

    if (IsInspectImminent() && _ButtonPressHelper.GetButtonPress("Inspect"))
      OnInspect();
  }

  bool IsInspectImminent() => IsPlayingAndNearby() && IsInspectable();
  bool IsPlayingAndNearby() => StateManager.Playing && _Nearby;

  public abstract void OnInspect();

  public virtual void OnPlayerOver() {
  }

  public virtual void OnPlayerOut() {
  }

  public virtual bool IsInspectable() {
    return true;
  }
}
