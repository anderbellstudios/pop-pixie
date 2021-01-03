using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedBlur : MonoBehaviour {
  public ScreenFade Fader;
  public Image BlurImage;
  public List<Material> BlurMaterials;
  public AnimationCurve Curve;
  public float Interval = 2f;

  private IntervalTimer Timer;

  void Awake() {
    Timer = new IntervalTimer() {
      Interval = Interval
    };
  }

  public void Activate() {
    if (!Timer.Started) {
      Timer.Reset();
      Fader.Flash("red", Interval);
    }
  }

  void Update() {
    Material material;

    if (Timer.Started) {
      material = BlurMaterials[(int) Mathf.Floor(
        Mathf.Clamp(
          BlurMaterials.Count * Curve.Evaluate(Timer.Progress()),
          0,
          BlurMaterials.Count - 1
        )
      )];

      if (Timer.Elapsed()) {
        Timer.Stop();
      }
    } else {
      material = BlurMaterials[0];
    }

    BlurImage.material = material;
  }
}
