using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingMenuEvents : AMenu {
  public AMenu OptionsMenu;
  public SceneChangeHopper SceneChangeHopper;
  public GameObject DebugModeIndicator;

  public override void LocalStart() {
    ResolutionData.Apply();
    DebugModeIndicator.SetActive(Debug.isDebugBuild);
  }

  public void Begin() {
    SceneChangeHopper.Hop();
  }

  public void Options() {
    OpenNestedMenu(OptionsMenu);
  }
}
