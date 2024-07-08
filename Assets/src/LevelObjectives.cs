using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectives : MonoBehaviour {
  public bool SingletonInstance = true;
  public static LevelObjectives Current;

  public bool GotKeycard = false, UsedAccessTerminal = false;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }
}
