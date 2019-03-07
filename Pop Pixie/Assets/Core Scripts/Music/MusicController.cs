using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

  public static MusicController Current;
  public AudioSource Player;

  private string SongName;

  void Awake () {
    if ( Current == null ) {
      Current = this;
    } else {
      Destroy( gameObject );
    }

    DontDestroyOnLoad( gameObject );
  }

  public void Play (AudioClip clip, string songName) { 
    if ( songName == SongName )
      return;

    Player.clip = clip;
    Player.Play();
    SongName = songName;
  }

}
