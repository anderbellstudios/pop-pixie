using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedBlur : MonoBehaviour {
  public ScreenFade Fader;
  public Image BlurImage;
  public MovementManager PlayerMovementManager;
  public List<Material> BlurMaterials;
  public AnimationCurve Curve;
  public float Interval = 2f;

  private IntervalTimer Timer;
  private float MovementModifier = 1f;

  void Awake() {
    Timer = new IntervalTimer() {
      Interval = Interval
    };

    PlayerMovementManager.SpeedModifiers.Add((s) => s * MovementModifier);
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
      float curveProgress = Curve.Evaluate(Timer.Progress());

      int materialIndex = (int) Mathf.Floor(
        Mathf.Clamp(
          BlurMaterials.Count * curveProgress,
          0,
          BlurMaterials.Count - 1
        )
      );

      material = BlurMaterials[materialIndex];

      MovementModifier = 1f - curveProgress;

      if (Timer.Elapsed()) {
        Timer.Stop();
      }
    } else {
      material = BlurMaterials[0];
      MovementModifier = 1f;
    }

    BlurImage.material = material;
  }
}
