using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LandingMenuEvents : AMenu {
  public AMenu OptionsMenu;
  public SceneChangeHopper SceneChangeHopper;
  public GameObject DebugModeIndicator;
  public TMP_Text EDCIndicatorText;

  public override void LocalStart() {
    ResolutionData.Apply();
    DebugModeIndicator.SetActive(Debug.isDebugBuild);

    if (EnhancedDataCollection.Enabled) {
      EDCIndicatorText.gameObject.SetActive(true);
      EDCIndicatorText.text += EnhancedDataCollection.ClientID;
    }
  }

  public void Begin() {
    SceneChangeHopper.Hop();
  }

  public void Options() {
    OpenNestedMenu(OptionsMenu);
  }
}
