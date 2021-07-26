using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingMenuEvents : AMenu {
  public AMenu OptionsMenu;
  public SceneChangeHopper SceneChangeHopper;

  public override void LocalStart() {
    ResolutionData.Apply();
  }

  public void Begin() {
    SceneChangeHopper.Hop();
  }

  public void Options() {
    OpenNestedMenu(OptionsMenu);
  }
}
