using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControllerTypeSelectionPhase : APhase {

  public TMP_Text TopText;
  public TMP_Text BottomText;
  public Image Image;
  public GameObject ControllerOptions;

  public override void LocalBegin() {
    TopText.text = "Controller Calibration";
    BottomText.text = "";
    ControllerOptions.active = true;
  }

  void ChooseController( string type ) {
    ControllerTypeData.SetType( type );
    PhaseFinished();
  }

  public override void AfterFinished() {
    ControllerOptions.active = false;
  }

  public void ChoosePS() {
    ChooseController("PS");
  }

  public void ChooseXbox() {
    ChooseController("Xbox");
  }

}
