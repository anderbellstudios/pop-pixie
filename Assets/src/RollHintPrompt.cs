using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollHintPrompt : MonoBehaviour {
  void Start() {
    InGamePrompt.Current.RegisterSource(98, () =>
      Roll.HasRolled
      ? null
      : "Press [Roll] while moving to <color=#ffff00>roll</color>"
    );
  }
}
