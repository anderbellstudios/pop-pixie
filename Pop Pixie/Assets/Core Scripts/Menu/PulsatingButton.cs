using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsatingButton : MonoBehaviour {
  public float FadeInDelay, FadeInDuration, PulseMinAlpha, PulseSpeed;
  public CanvasGroup FadeGroup;

  private bool FadingIn = false;
  private float BaseAlpha = 0f;

  void Start() {
    Invoke( "BeginFadeIn", FadeInDelay );
  }

  void BeginFadeIn() {
    FadingIn = true;
  }

  void Update() {
    if ( FadingIn )
      BaseAlpha += Time.deltaTime / FadeInDuration;

    FadeGroup.alpha = Alpha();
  }

  float Alpha() {
    return Mathf.Clamp( BaseAlpha, 0f, 1f ) * PulseAlpha();
  }

  float PulseAlpha() {
    float normal = 0.5f * ( 1f - PulseMinAlpha );
    float sin = Mathf.Sin( Time.time * PulseSpeed );

    return ( 1f - normal ) + ( normal * sin );
  }


}
