using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessTerminalIndicatorLight : MonoBehaviour {
  public GameObject IndicatorLight;

  public float BlinksPerSecond;
  public float IntermittentBlinkInterval;
  public float IntermittentBlinkDuration;

  private bool ExternalBlinking = false;
  public void SetBlinking(bool blinking) => ExternalBlinking = blinking;

  private bool IntermittentBlinking = false;
  private bool IsBlinking() => ExternalBlinking || IntermittentBlinking;

  void Start() {
    AsyncTimer.BaseTime.SetInterval(() => {
      IntermittentBlinking = true;

      AsyncTimer.BaseTime.SetTimeout(() => {
        IntermittentBlinking = false;
      }, IntermittentBlinkDuration);
    }, IntermittentBlinkInterval);
  }

  void Update() {
    if (!IsBlinking()) {
      IndicatorLight.SetActive(false);
      return;
    }

    float blinkInterval = 1f / BlinksPerSecond;
    float modTime = Time.time % blinkInterval;
    IndicatorLight.SetActive(modTime < blinkInterval / 2f);
  }
}
