using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandingMenuBackground : MonoBehaviour {
  public float Duration;

  public Transform Transform;
  public AnimationCurve AltitudeCurve;
  public float AltitudeAmplitude;

  public Image Image;
  public AnimationCurve OpacityCurve;

  public List<Sprite> Images;

  private Stopwatch AnimationStopwatch;
  private int CurrentImageIndex = -1;

  void Start() {
    AnimationStopwatch = new Stopwatch.BaseTime();
    NextImage();
  }

  void Update() {
    float progress = AnimationStopwatch.Progress(Duration);

    float altitude = AltitudeCurve.Evaluate(progress) * AltitudeAmplitude;
    Transform.localPosition = new Vector3(0, altitude, 0);

    float opacity = OpacityCurve.Evaluate(progress);
    Color color = Image.color;
    color.a = opacity;
    Image.color = color;

    if (progress >= 1f) {
      AnimationStopwatch.Reset();
      NextImage();
    }
  }

  private void NextImage() {
    CurrentImageIndex = (CurrentImageIndex + 1) % Images.Count;
    Image.sprite = Images[CurrentImageIndex];
  }
}
