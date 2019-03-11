using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSounds : MonoBehaviour {
  public SoundController SoundPlayer;
  public List<AudioClip> Sounds;
  public float MinInterval, MaxInterval;

  private DateTime LastPlayed;
  private float PlayInterval;

  void Start () {
    UpdatePlayInterval();
  }

  void Update () {
    if ( ShouldPlay() ) {
      // Because C# doesn't appear to have any built-in 
      // random sampling methods. 
      int i = UnityEngine.Random.Range(0, Sounds.Count);
      var sound = Sounds[i];
      SoundPlayer.Play(sound);

      UpdatePlayInterval();
    }
  }

  bool ShouldPlay () {
    var since = DateTime.Now.Subtract( LastPlayed ).TotalSeconds;
    return since > PlayInterval;
  }

  void UpdatePlayInterval () {
    LastPlayed = DateTime.Now;

    PlayInterval = MinInterval 
      + ( MaxInterval - MinInterval ) 
      * ( UnityEngine.Random.value );
  }
}
