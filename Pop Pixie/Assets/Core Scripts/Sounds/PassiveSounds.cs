using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSounds : MonoBehaviour {
  public EnemyHitPointEvents EnemyHitPointEvents;
  public SoundController SoundPlayer;
  public List<AudioClip> Sounds;
  public float MinInterval, MaxInterval;

  private IntervalTimer Timer;

  void Start () {
    Timer = new IntervalTimer() {
      TimeClass = "PlayingTime",
    };

    UpdatePlayInterval();
  }

  void Update () {
    if ( StateManager.Isnt( State.Playing ) ) return;
    Timer.IfElapsed( PlaySound );
  }

  void PlaySound () {
    if ( EnemyHitPointEvents.IsDead ) return;

    // Because C# doesn't appear to have any built-in 
    // random sampling methods. 
    int i = UnityEngine.Random.Range(0, Sounds.Count);
    var sound = Sounds[i];
    SoundPlayer.Play(sound);

    UpdatePlayInterval();
  }

  void UpdatePlayInterval () {
    Timer.Interval = MinInterval 
      + ( MaxInterval - MinInterval ) 
      * ( UnityEngine.Random.value );

    Timer.Reset();
  }
}
