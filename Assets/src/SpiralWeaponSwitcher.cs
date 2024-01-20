using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * - Weapons are rendered on an infinite helix in 3D space.
 * - Near Z is 0.5, far Z is 1.5, center Z is 1.0.
 */

public class SpiralWeaponSwitcher : MonoBehaviour {
  public GameObject BaseIndicator;
  public int IndicatorCount;
  public float StartRevolutions;
  public float EndRevolutions;
  public float Radius;
  public float Stretch;
  public float JoystickThreshold;

  private Vector2 PreviousJoystick = Vector2.right;
  private float TargetAngle, CurrentAngle = 0f;
  private List<GameObject> Indicators = new List<GameObject>();

  void Update() {
    Vector2 joystick = new Vector2(
      WrappedInput.GetAxis("Horizontal"),
      WrappedInput.GetAxis("Vertical")
    );

    if (joystick.magnitude > JoystickThreshold) {
      float angle = Vector2.SignedAngle(PreviousJoystick, joystick) * Mathf.Deg2Rad;
      TargetAngle += angle;
      PreviousJoystick = joystick;
    }

    CurrentAngle = Mathf.Lerp(CurrentAngle, TargetAngle, 0.3f);

    Indicators.ForEach(Destroy);

    float startAngle = StartRevolutions * 2 * Mathf.PI;
    float endAngle = EndRevolutions * 2 * Mathf.PI;

    Func<int, float> nthAngle = (int n) => {
      float progress = (float)n / IndicatorCount;
      return Mathf.Lerp(startAngle, endAngle, progress);
    };

    float angleStep = nthAngle(1) - nthAngle(0);

    for (int i = 0; i < IndicatorCount; i++) {
      GameObject indicator = Instantiate(BaseIndicator, transform);
      indicator.SetActive(true);

      float angle = nthAngle(i);

      Vector3 position = HelixPosition(
        radius: Radius,
        scaleZ: Stretch,
        centerAngle: CurrentAngle,
        angle: angle
      );

      if (position.z < 0.5f || position.z > 1.5f) {
        Destroy(indicator);
        continue;
      }

      indicator.transform.localPosition = Perspective(position);

      float scale = 1f / position.z;
      indicator.transform.localScale = Vector3.one * scale;

      Image image = indicator.GetComponent<Image>();

      float opacity = Mathf.Clamp01(1f - Mathf.Abs(position.z - 1f) * 2f);
      image.color = new Color(1f, 1f, 1f, opacity);

      if (Mathf.Abs(TargetAngle - angle) < angleStep / 2f) {
        image.color = Color.red;
      }

      Indicators.Add(indicator);
    }
  }

  Vector3 HelixPosition(float radius, float scaleZ, float centerAngle, float angle) => new Vector3(
    radius * Mathf.Cos(angle),
    radius * Mathf.Sin(angle),
    (scaleZ * (angle - centerAngle)) + 1f // 1.0 when centered
  );

  Vector3 Perspective(Vector3 p) => new Vector3(
    p.x / p.z,
    p.y / p.z,
    p.z // For ease of debugging
  );
}
