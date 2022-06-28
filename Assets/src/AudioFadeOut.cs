using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour {

  public bool SingletonInstance = true;
  public static AudioFadeOut Current;

  private IntervalTimer FadeOutTimer;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    FadeOutTimer = new IntervalTimer();
  }

  public void FadeOut( float duration ) {
    FadeOutTimer.Interval = duration;
    FadeOutTimer.Reset();
  }

  public float FadeLevel() {
    if ( FadeOutTimer.Started ) {
      return 1f - FadeOutTimer.Progress();
    } else {
      return 1f;
    }
  }

}
