using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {
  public bool MenuSoundEnabled = true;
  public bool NavigationEnabled = true;
  [TextArea]
  public string Explanation;
  public Button OverrideUp, OverrideDown, OverrideLeft, OverrideRight;

  public AMenu Menu { get; private set; }

  public void SetMenu(AMenu menu) {
    Menu = menu;
  }

  void OnEnable() {
    Menu?.ActiveButtonsChanged();
  }

  void OnDisable() {
    Menu?.ActiveButtonsChanged();
  }
}
