using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour {
  public AudioSource IntroAudioSource, MainAudioSource;
  public AudioLowPassFilter IntroLowPassFilter, MainLowPassFilter;

  Song CurrentSong = null;

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

  void Start() {
    StateManager.AddListener(() => {
      IntroLowPassFilter.enabled = MainLowPassFilter.enabled = StateManager.Enabled(StateFeatures.MuffleMusic);
    });
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

        double introEndTime = AudioSettings.dspTime + song.IntroClip.length;

        IntroAudioSource.clip = song.IntroClip;
        IntroAudioSource.Play();
        IntroAudioSource.SetScheduledEndTime(introEndTime);

        MainAudioSource.clip = song.AudioClip;
        MainAudioSource.PlayScheduled(introEndTime);
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

  void Update() {
    IntroAudioSource.volume = MainAudioSource.volume =
      AudioFadeOut.Current.FadeLevel(true) * ((float) OptionsData.MusicVolume);

    if (CurrentSong != null && CurrentSong.Resume)
      SongPlaybackTimeData.Record(CurrentSong, MainAudioSource.timeSamples);
  }
}
