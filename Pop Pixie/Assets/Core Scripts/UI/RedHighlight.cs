using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedHighlight : MonoBehaviour {

  public float Duration;

  private Image image;

	// Use this for initialization
	void Start () {
    image = gameObject.GetComponent<Image>();
	}

  public void Flash () {
    image.enabled = true;
    image.CrossFadeAlpha( 1.0f, 0.0f, false );
    image.CrossFadeAlpha( 0.0f, Duration, false );
  }

}
