using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CanvasFadeInOut : MonoBehaviour {
  public CanvasGroup CanvasGroup;
  public bool BeforeInitialVisible = false;
  public bool InitialVisible = true;
  public float Duration = 0.1f;

  private float TargetOpacity;

  void OnEnable() {
    Opacity = BeforeInitialVisible ? 1f : 0f;
    TargetOpacity = InitialVisible ? 1f : 0f;
  }

  public void Fade(float target) {
    TargetOpacity = target;
  }

  void Update() {
    Opacity = Mathf.MoveTowards(Opacity, TargetOpacity, Time.deltaTime / Duration);
  }

  float Opacity {
    get {
      return CanvasGroup.alpha;
    }

    set {
      CanvasGroup.alpha = value;
    }
  }
}
