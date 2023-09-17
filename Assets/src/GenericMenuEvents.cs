﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMenuEvents : MonoBehaviour {

  public ScreenFade Fader;

  public float FadeInDelay, FadeInDuration, FadeOutDuration, PostFadeOutDelay;

  void Start() {
    Fader.Fade("to black", 0.0f);
    Invoke("FadeIn", FadeInDelay);
    LocalStart();
  }

  public virtual void LocalStart() { }

  private bool FadingIn = true;

  void FadeIn() {
    FadingIn = true;
    Fader.Fade("from black", FadeInDuration);
    Invoke("_AfterFadeIn", FadeInDuration);
  }

  void _AfterFadeIn() {
    FadingIn = false;
    AfterFadeIn();
  }

  public virtual void AfterFadeIn() { }

  private bool FadingOut = false;
  private Action FadeOutCallback;

  public void FadeOut(Action callback) {
    if (FadingIn || FadingOut)
      return; // Don't interrupt a fade in or fade out

    FadingOut = true;
    FadeOutCallback = callback;

    AudioFadeOut.Current.FadeOut(FadeOutDuration);
    Fader.Fade("to black", FadeOutDuration);
    Invoke("CallFadeOutCallback", FadeOutDuration + PostFadeOutDelay);
  }

  void CallFadeOutCallback() {
    FadeOutCallback();
  }
}
