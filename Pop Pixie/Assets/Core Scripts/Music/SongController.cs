using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour {

  public AudioSource AudioSource;

  Song CurrentSong = null;

  public static SongController Current;

  void Awake() {
    Current = this;
  }

  public void Play( Song song ) {
    RecordPlaybackTime();

    AudioSource.clip = song.AudioClip;
    AudioSource.timeSamples = SongPlaybackTimeData.Fetch(song);
    AudioSource.Play();

    CurrentSong = song;
  }

  public void RecordPlaybackTime() {
    if ( CurrentSong == null ) return;
    SongPlaybackTimeData.Record( CurrentSong, AudioSource.timeSamples );
  }

}
