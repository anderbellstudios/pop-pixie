using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OscillateBrightness : MonoBehaviour {
  public Image Image;
  public SpriteRenderer SpriteRenderer;
  public AnimationCurve AnimationCurve;

  public float MinSpeed, MaxSpeed;
  private float Offset, Speed;
  private Color InitialColor;

  public void SetColor(Color color) {
    InitialColor = color;

    if (!this.enabled || TestMode.Enabled)
      UpdateColor(color);
  }

  void Awake() {
    Offset = Random.Range(0, 1);
    Speed = Random.Range(MinSpeed, MaxSpeed);
    InitialColor = (Color)(Image?.color ?? SpriteRenderer?.color);
  }

  void Update() {
    if (TestMode.Enabled)
      return;

    float t = (Time.time + Offset) * Speed;
    float v = AnimationCurve.Evaluate(t % 1);

    Color color = new Color(
      InitialColor.r * v,
      InitialColor.g * v,
      InitialColor.b * v,
      InitialColor.a
    );

    UpdateColor(color);
  }

  private void UpdateColor(Color color) {
    if (Image) {
      Image.color = color;
    }

    if (SpriteRenderer) {
      SpriteRenderer.color = color;
    }
  }
}
