using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScheduler : MonoBehaviour {

  public float FireInterval;
  public float RespiteInterval;

  public List<Laser> Lasers;

  private IntervalTimer Timer;

  // -1 is the respite phase; anything else corresponds to an index of Lasers
  private int Phase = -1;

  public void RemoveLaser( Laser laser ) {
    Lasers.Remove(laser);
    Phase = -1;
  }

  void OnEnable() {
    Timer = new IntervalTimer();

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
    if ( Phase == Lasers.Count )
      Phase = -1;

    if ( !RespitePhase() ) {
      var laser = Lasers[Phase];
      laser.BeginFiring();
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
