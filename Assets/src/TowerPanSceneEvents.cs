using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TowerPanSceneEvents : MonoBehaviour {
  public Transform ContainerTransform;
  public Image DarkenBehind, DarkenFront;
  public PlayCaptionLine PlayCaptionLine;
  public AnimationCurve PanCurve, DarkenBehindCurve, DarkenFrontCurve;
  public float
    DelayBeforePan, PanDuration, PanFrom, PanTo,
    DelayBeforeCaptionLine,
    DelayBeforeDarkenBehind, DarkenBehindDuration,
    DelayBeforeDarkenFront, DarkenFrontDuration,
    DelayBeforeComplete;
  public UnityEvent OnComplete;

  private float TimeElapsed = 0;
  private bool PlayedCaptionLine = false;
  private bool IsComplete = false;

  void Update() {
    TimeElapsed += Time.deltaTime;

    float panProgress = Mathf.Clamp01((TimeElapsed - DelayBeforePan) / PanDuration);
    float elapsedSincePan = TimeElapsed - DelayBeforePan - PanDuration;

    if (!PlayedCaptionLine && TimeElapsed > DelayBeforeCaptionLine) {
      PlayedCaptionLine = true;
      PlayCaptionLine.Perform();
    }

    float darkenBehindProgress = Mathf.Clamp01((elapsedSincePan - DelayBeforeDarkenBehind) / DarkenBehindDuration);
    float darkenFrontProgress = Mathf.Clamp01((elapsedSincePan - DelayBeforeDarkenFront) / DarkenFrontDuration);
    float elapsedSinceDarken = elapsedSincePan - DelayBeforeDarkenFront - DarkenFrontDuration;

    ContainerTransform.localPosition = new Vector3(
      ContainerTransform.localPosition.x,
      Mathf.Lerp(PanFrom, PanTo, PanCurve.Evaluate(panProgress)),
      ContainerTransform.localPosition.z
    );

    DarkenBehind.color = new Color(
      DarkenBehind.color.r,
      DarkenBehind.color.g,
      DarkenBehind.color.b,
      DarkenBehindCurve.Evaluate(darkenBehindProgress)
    );

    DarkenFront.color = new Color(
      DarkenFront.color.r,
      DarkenFront.color.g,
      DarkenFront.color.b,
      DarkenFrontCurve.Evaluate(darkenFrontProgress)
    );

    if (!IsComplete && elapsedSinceDarken > DelayBeforeComplete) {
      IsComplete = true;
      OnComplete.Invoke();
    }
  }
}
