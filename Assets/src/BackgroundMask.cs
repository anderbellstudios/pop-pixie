using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMask : MonoBehaviour {
  public CanvasFadeInOut CanvasFadeInOut;

  public void AppearSuddenly() {
    CanvasFadeInOut.SetOpacity(0f);
  }

  public void FadeIn() {
    CanvasFadeInOut.Fade(0f);
  }
}
