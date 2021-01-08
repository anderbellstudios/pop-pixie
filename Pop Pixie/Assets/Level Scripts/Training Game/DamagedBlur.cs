using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedBlur : MonoBehaviour {
  public static DamagedBlur Current;

  public ScreenFade Fader;
  public Image BlurImage;
  public GameObject BlurImageGameObject;
  public MovementManager PlayerMovementManager;
  public List<Material> BlurMaterials;
  public AnimationCurve Curve;
  public float Interval = 2f;

  private IntervalTimer Timer;
  private float MovementModifier = 1f;

  void Awake() {
    Current = this;

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

    if (Timer.Started) {
      float curveProgress = Curve.Evaluate(Timer.Progress());

      int materialIndex = (int) Mathf.Floor(
        Mathf.Clamp(
          BlurMaterials.Count * curveProgress,
          0,
          BlurMaterials.Count - 1
        )
      );

      BlurImageGameObject.SetActive(true);
      BlurImage.material = BlurMaterials[materialIndex];

      MovementModifier = 1f - curveProgress;

      if (Timer.Elapsed()) {
        Timer.Stop();
      }
    } else {
      BlurImageGameObject.SetActive(false);
      BlurImage.material = BlurMaterials[0];
      MovementModifier = 1f;
    }
  }
}
