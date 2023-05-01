using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour {
  public AudioSource IntroAudioSource, MainAudioSource;

  Song CurrentSong = null;
  double IntroLength = 0;

  public bool SingletonInstance = true;
  public static SongController Current;

  void Awake() {
    if (SingletonInstance) {
      if (Current != null) {
        Destroy(gameObject);
        return;
      }

      Current = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  public void Play(Song song) {
    if (song.Equals(CurrentSong))
      return;

    if (song == null) {
      IntroAudioSource.Stop();
      MainAudioSource.Stop();
    } else {
      if (song.HasIntro) {
        if (song.Resume)
          throw new System.Exception("Resume is not supported for songs with intro");

        MainAudioSource.Stop();

        IntroLength = song.IntroClip.length;
        IntroAudioSource.clip = song.IntroClip;
        IntroAudioSource.Play();

        Invoke("EnqueueMainClip", song.IntroClip.length / 2f);

        MainAudioSource.clip = song.AudioClip;
      } else {
        IntroAudioSource.Stop();
        MainAudioSource.clip = song.AudioClip;

        if (song.Resume)
          MainAudioSource.timeSamples = SongPlaybackTimeData.Fetch(song);

        MainAudioSource.Play();
      }
    }

    CurrentSong = song;
  }

  void EnqueueMainClip() {
    if (IntroAudioSource.isPlaying) {
      // Minimise delay between fetching these time-sensitive values
      double virtualTransitionTime = AudioSettings.dspTime - IntroAudioSource.time;
      double transitionTime = IntroLength + virtualTransitionTime;
      IntroAudioSource.SetScheduledEndTime(transitionTime);
      MainAudioSource.PlayScheduled(transitionTime);
    }
  }

  void Update() {
    IntroAudioSource.volume = MainAudioSource.volume =
      AudioFadeOut.Current.FadeLevel(true) * ((float) OptionsData.MusicVolume);

    if (CurrentSong != null && CurrentSong.Resume)
      SongPlaybackTimeData.Record(CurrentSong, MainAudioSource.timeSamples);
  }
}
