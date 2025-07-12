using System;
using UnityEngine;

public class MenuButton : MonoBehaviour {
  public bool MenuSoundEnabled = true;
  public bool NavigationEnabled = true;

  [TextArea]
  public string Explanation;

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
