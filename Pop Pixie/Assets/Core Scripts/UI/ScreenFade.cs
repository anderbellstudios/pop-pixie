using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

  public float FlashDuration;
  public float FadeDuration;

  private Image image;

	// Use this for initialization
	void Start () {
    image = gameObject.GetComponent<Image>();
	}

  public void Flash (string colour) {
    switch (colour) {
      case "red":
        Flash( new Color32(255, 0, 0, 100) );
        break;
    }
  }

  public void Flash (Color32 colour) {
    Fade(1.0f, 0.0f, colour, FlashDuration);
  }

  public void Fade(string fade) {
    switch (fade) {
      case "to black":
        Fade(
          0.0f, 
          1.0f, 
          new Color32(0, 0, 0, 255),
          FadeDuration
        );
        break;
    }
  }

  public void Fade(float u, float v, Color32 colour, float duration) {
    image.color = colour;
    image.CrossFadeAlpha( u, 0.0f, false );
    image.CrossFadeAlpha( v, duration, false );
  }
}
