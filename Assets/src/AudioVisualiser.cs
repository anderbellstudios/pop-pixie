using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualiser : MonoBehaviour {
  public Transform BarContainer;
  public AudioSource AudioSource;
  public float Sensitivity = 1;
  public AnimationCurve EQ = AnimationCurve.Constant(0, 63, 1);
  public float DecayRate = 30;

  private RectTransform[] BarTransforms = new RectTransform[64];
  private float[] SmoothedSamples = new float[64];

  void Awake() {
    int barCount = BarContainer.childCount;

    if (Debug.isDebugBuild) {
      if (barCount != 64) {
        throw new Exception("AudioVisualiser: BarContainer must have 64 children");
      }
    }

    for (int i = 0; i < barCount; i++) {
      BarTransforms[i] = BarContainer.GetChild(i).GetComponent<RectTransform>();
    }
  }

  void Update() {
    float[] spectrum = new float[64];

    AudioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

    for (int i = 0; i < 64; i++) {
      float instantaneous = Mathf.Clamp(
        Mathf.Log(spectrum[i] + 1) *
          EQ.Evaluate(i) *
          Sensitivity,
        0.01f,
        1f
      );

      float smoothed = SmoothedSamples[i] = Math.Max(
        instantaneous,
        Mathf.Lerp(
          SmoothedSamples[i],
          instantaneous,
          Time.deltaTime * DecayRate
        )
      );

      BarTransforms[i].localScale = new Vector3(1, smoothed, 1);
    }
  }
}
