using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

  public LaserBeam LaserBeam;
  public float InitialDelay;
  public float FireInterval;
  public float SweepDuration;
  public float InitialAngle, FinalAngle;
  public bool Running;

  private IntervalTimer FireTimer;
  private IntervalTimer SweepTimer;

  void Start() {
    FireTimer = new IntervalTimer() {
      Interval = FireInterval
    };

    SweepTimer = new IntervalTimer() {
      Interval = SweepDuration
    };
  }

  void StartFireTimer() {
    FireTimer.Start();
  }

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( !Running ) {
      Invoke("StartFireTimer", InitialDelay);
      Running = true;
    }

    FireTimer.IfElapsed(
      () => BeginFiring()
    );

    SweepTimer.UnlessElapsed(
      () => FireBeam()
    );
  }

  void BeginFiring() {
    SweepTimer.Reset();
  }

  void FireBeam() {
    LaserBeam.Fire(
      BeamAngle()
    );
  }

  float BeamAngle() {
    return Mathf.Lerp(
      InitialAngle,
      FinalAngle,
      SweepProgress()
    );
  }

  float SweepProgress() {
    return SweepTimer.TimeSinceElapsed() / SweepDuration;
  }

}
