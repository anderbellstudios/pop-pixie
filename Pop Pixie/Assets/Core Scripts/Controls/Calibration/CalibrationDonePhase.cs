using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalibrationDonePhase : APhase {

  public TMP_Text TopText;
  public TMP_Text BottomText;
  public Image Image;

  public Sprite DoneImage;

  public override void LocalBegin() {
    Image.enabled = true;

    TopText.text = "Calibration complete";
    BottomText.text = "Press escape to exit.";
    Image.sprite = DoneImage;
  }

}
