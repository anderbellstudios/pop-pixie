using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopPixie {
  namespace Audio {
    public class AudioMixer : MonoBehaviour {

      public bool SingletonInstance = true;
      public static AudioMixer Current;

      public AudioSource AudioSource;

      private LowPriorityBehaviour LowPriorityBehaviour;
      private IntervalTimer FadeOutTimer;

      void Awake() {
        if (SingletonInstance)
          Current = this;

        LowPriorityBehaviour = new LowPriorityBehaviour();

        FadeOutTimer = new IntervalTimer();
      }

      void Update() {
        if (FadeOutTimer.Started) {
          UpdateVolume();
        } else {
          LowPriorityBehaviour.EveryNFrames(20, UpdateVolume); 
        }
      }

      void UpdateVolume() {
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
  }
}
