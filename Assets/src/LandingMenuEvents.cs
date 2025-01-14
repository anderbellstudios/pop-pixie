using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LandingMenuEvents : AMenu {
  public AMenu OptionsMenu;
  public SceneChangeHopper SceneChangeHopper;
  public TMP_Text DebugModeIndicator, EDCIndicator;

  public static bool FirstTime = true;

  public override void LocalStart() {
    if (FirstTime) {
      ResolutionData.Apply();
      AudioOutput.Initialise();
      FirstTime = false;
    }

    if (Debug.isDebugBuild) {
      DebugModeIndicator.gameObject.SetActive(true);

      String branchName = BuildMetaData.BranchName;
      String commitHash = BuildMetaData.CommitHash;

      if (branchName != null && commitHash != null) {
        DebugModeIndicator.text += $" ({branchName}, {commitHash.Substring(0, 7)})";
      }
    }

    if (EnhancedDataCollection.Enabled) {
      EDCIndicator.gameObject.SetActive(true);
      EDCIndicator.text += EnhancedDataCollection.ClientID;
    }
  }

  public void Begin() {
    SceneChangeHopper.Hop();
  }

  public void Options() {
    OpenNestedMenu(OptionsMenu);
  }
}
