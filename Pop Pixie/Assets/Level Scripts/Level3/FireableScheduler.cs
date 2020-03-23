using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireableScheduler : MonoBehaviour {

  public delegate void FireableEvent();
  public event FireableEvent FireableRemoved = delegate {};
  public event FireableEvent CycleFinished = delegate {};

  public float FireInterval;
  public float RespiteInterval;

  public List<AFireable> Fireables;

  private IntervalTimer Timer;

  // -1 is the respite phase; anything else corresponds to an index of Fireables
  private int Phase = -1;

  public void RemoveFireable( AFireable fireable ) {
    FireableRemoved();

    int fireable_index = Fireables.FindIndex( l => l == fireable );

    // Make sure that the fireable about to be fired next remains the same
    if ( fireable_index < Phase )
      Phase -= 1;

    Fireables.Remove(fireable);
    fireable.StopFiring();
  }

  void OnEnable() {
    Timer = new IntervalTimer() {
      TimeClass = "PlayingTime"
    };

    Timer.Start();
  }

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( RespitePhase() ) {
      Timer.Interval = RespiteInterval;
    } else {
      Timer.Interval = FireInterval;
    }

    Timer.IfElapsed( () => RunPhase() );
  }

  void RunPhase() {
    if ( Phase >= Fireables.Count ) {
      Phase = -1;
      CycleFinished();
    }

    if ( !RespitePhase() ) {
      var fireable = Fireables[Phase];
      fireable.BeginFiring();
    }

    Phase += 1;
  }

  bool RespitePhase() {
    return Phase == -1;
  }

  float TimerInterval() {
    return FireInterval;
  }

}
