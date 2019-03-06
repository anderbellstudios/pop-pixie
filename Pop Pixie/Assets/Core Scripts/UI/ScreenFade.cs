using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

  private Image image;

	// Use this for initialization
	void Start () {
    image = gameObject.GetComponent<Image>();
	}

  public void Flash (string colour, float duration) {
    switch (colour) {
      case "red":
        Flash( new Color32(255, 0, 0, 100), duration );
        break;
    }
  }

  public void Flash (Color32 colour, float duration) {
    Fade(1.0f, 0.0f, colour, duration);
  }

  public void Fade(string fade, float duration) {
    switch (fade) {
      case "to black":
        Fade(
          0.0f, 
          1.0f, 
          new Color32(0, 0, 0, 255),
          duration
        );
        break;

      case "from black":
        Fade(
          1.0f, 
          0.0f, 
          new Color32(0, 0, 0, 255),
          duration
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
