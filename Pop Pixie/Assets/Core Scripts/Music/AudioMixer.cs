using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer : MonoBehaviour {

  public AudioSource AudioSource;

  IntervalTimer FadeOutTimer;

  public static AudioMixer Current;

  void Awake() {
    Current = this;
    FadeOutTimer = new IntervalTimer();
  }

  void Update() {
    AudioSource.volume = Volume();
  }

  public void FadeOut( float duration ) {
    FadeOutTimer.Interval = duration;
    FadeOutTimer.Reset();
  }

  float Volume() {
    return FadeLevel() * (float) OptionsData.MusicVolume;
  }

  float FadeLevel() {
    if ( FadeOutTimer.Started ) {
      return 1f - FadeOutTimer.Progress();
    } else {
      return 1f;
    }
  }

}
