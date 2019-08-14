using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

  public LaserBeam LaserBeam;
  public float FireInterval;
  public float SweepDuration;
  public float InitialAngle, FinalAngle;

  private IntervalTimer FireTimer;
  private IntervalTimer SweepTimer;

  void Start() {
    FireTimer = new IntervalTimer() {
      Interval = FireInterval
    };

    FireTimer.Start();

    SweepTimer = new IntervalTimer() {
      Interval = SweepDuration
    };
  }

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

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
