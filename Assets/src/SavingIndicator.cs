using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingIndicator : MonoBehaviour {
  public CanvasFadeInOut CanvasFadeInOut;
  public float ShowDelay, HideDelay;

  public void Saved() {
    AsyncTimer.BaseTime.SetTimeout(() => {
      CanvasFadeInOut.Fade(1f);

      AsyncTimer.BaseTime.SetTimeout(() => {
        CanvasFadeInOut.Fade(0f);
      }, HideDelay);
    }, ShowDelay);
  }
}
