using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AInspectable : MonoBehaviour {

  public static Dictionary<AInspectable, bool> IsNearby = new Dictionary<AInspectable, bool>() {};

  bool _Nearby;
  bool _ButttonReleased;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      _Nearby = true;
    }
  }

  void OnTriggerExit2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      _Nearby = false;
    }
  }

  void Update() {
    bool statePlaying = StateManager.Is( State.Playing );

    IsNearby[this] = statePlaying && _Nearby;

    if ( statePlaying && _ButttonReleased && _Nearby && WrappedInput.GetButtonDown("Inspect") )
      OnInspect();

    _ButttonReleased = statePlaying && ! WrappedInput.GetButton("Inspect");
  }

  public static bool ShowButtonPrompt() {
    return IsNearby.Values.Any( isTrue => isTrue );
  }

  public abstract void OnInspect();

}
