using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessAudioSource : MonoBehaviour {
  public AudioSource PrimaryAudioSource, SecondaryAudioSource;

  private Action OnPlay = null;
  private bool AwaitingTransition = false;
  private double TransitionTime = 0;

  public AudioSource PlayScheduled(AudioClip clip, double time, Action onPlay) {
    SecondaryAudioSource.clip = clip;
    PrimaryAudioSource.SetScheduledEndTime(time);
    SecondaryAudioSource.PlayScheduled(time);
    OnPlay = onPlay;
    AwaitingTransition = true;
    TransitionTime = time;
    return SecondaryAudioSource;
  }

  public AudioSource Play(AudioClip clip, Action onPlay) {
    return PlayScheduled(clip, AudioSettings.dspTime, onPlay);
  }

  public AudioSource PlayNext(AudioClip clip, Action onPlay) {
    AudioClip currentClip = PrimaryAudioSource.clip;

    if (!currentClip) {
      return Play(clip, onPlay);
    }

    return PlayScheduled(
      clip,
      AudioSettings.dspTime + currentClip.length - PrimaryAudioSource.time,
      onPlay
    );
  }

  public void Stop() {
    PrimaryAudioSource.Stop();
    SecondaryAudioSource.Stop();
  }

  public void ForEachAudioSource(Action<AudioSource> action) {
    action(PrimaryAudioSource);
    action(SecondaryAudioSource);
  }

  void Update() {
    if (AwaitingTransition && AudioSettings.dspTime >= TransitionTime) {
      AwaitingTransition = false;
      SwapAudioSources();
      OnPlay?.Invoke();
    }
  }

  private void SwapAudioSources() {
    AudioSource temp = PrimaryAudioSource;
    PrimaryAudioSource = SecondaryAudioSource;
    SecondaryAudioSource = temp;
  }
}
