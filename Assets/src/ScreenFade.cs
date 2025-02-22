using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {
  public bool SingletonInstance = true;
  public static ScreenFade Current;

  private Image Image;

  void Awake() {
    if (SingletonInstance) {
      if (Current != null) {
        throw new System.Exception("Cannot have multiple singleton instances of ScreenFade");
      }
      Current = this;
    }

    Image = gameObject.GetComponent<Image>();
  }

  public static void DamageFlash() {
    Fade(0.4f, 0f, Color.red, 2f);
  }

  public static void FadeIn(float duration) {
    Fade(1f, 0f, Color.black, duration);
  }

  public static void FadeOut(float duration) {
    Fade(0f, 1f, Color.black, duration);
  }

  private static void Fade(float fromOpacity, float toOpacity, Color color, float duration) {
    Current.Image.color = color;
    Current.Image.CrossFadeAlpha(fromOpacity, 0f, false);
    Current.Image.CrossFadeAlpha(toOpacity, duration, false);
  }
}
