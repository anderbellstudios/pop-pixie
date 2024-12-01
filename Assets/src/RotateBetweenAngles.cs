using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBetweenAngles : MonoBehaviour {
  public List<float> Angles;
  public bool Smoothing = true;
  public float Speed = 30f;
  public float PauseDuration = 0f;

  private int NextAngleIndex = 1;
  private float LocalTime = 0f;

  void Update() {
    if (!StateManager.Playing)
      return;

    // Do not advance time if behaviour is disabled
    LocalTime += Time.deltaTime;

    float previousAngle = Angles[PreviousAngleIndex];
    float nextAngle = Angles[NextAngleIndex];
    float angularDistance = Mathf.Abs(nextAngle - previousAngle);

    float duration = angularDistance / Speed;
    float progress = Mathf.Clamp01(LocalTime / duration);

    Angle = Smoothing
      ? Mathf.SmoothStep(previousAngle, nextAngle, progress)
      : Mathf.Lerp(previousAngle, nextAngle, progress);

    float timeAfterEnd = LocalTime - duration;

    if (timeAfterEnd >= PauseDuration) {
      LocalTime = 0f;
      NextAngleIndex = (NextAngleIndex + 1) % Angles.Count;
    }
  }

  private int PreviousAngleIndex => NextAngleIndex == 0
    ? Angles.Count - 1
    : NextAngleIndex - 1;

  private float Angle {
    set { transform.localRotation = Quaternion.Euler(0, 0, value); }
  }
}
