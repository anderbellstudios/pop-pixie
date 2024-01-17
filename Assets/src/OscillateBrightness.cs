using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OscillateBrightness : MonoBehaviour {
  public Image Image;
  public AnimationCurve AnimationCurve;

  public float MinSpeed, MaxSpeed;
  private float Offset, Speed;
  private Color InitialColor;

  void Start() {
    Offset = Random.Range(0, 1);
    Speed = Random.Range(MinSpeed, MaxSpeed);
    InitialColor = Image.color;
  }

  void Update() {
    float t = (Time.time + Offset) * Speed;
    float v = AnimationCurve.Evaluate(t % 1);

    Image.color = new Color(
      InitialColor.r * v,
      InitialColor.g * v,
      InitialColor.b * v,
      InitialColor.a
    );
  }
}
