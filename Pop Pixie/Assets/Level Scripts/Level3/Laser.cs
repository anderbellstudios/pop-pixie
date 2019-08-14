using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

  public LaserBeam LaserBeam;
  public LineRenderer LineRenderer;
  // public float InitialDelay;
  // public float FireInterval;
  public float SweepDuration;
  public float InitialAngle, FinalAngle;
  public bool Running;

  // private IntervalTimer FireTimer;
  private IntervalTimer SweepTimer;

  void Start() {
    // FireTimer = new IntervalTimer() {
    //   Interval = FireInterval
    // };

    SweepTimer = new IntervalTimer() {
      Interval = SweepDuration
    };
  }

  void StartFireTimer() {
    // FireTimer.Start();
  }

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    // if ( !Running ) {
    //   Invoke("StartFireTimer", InitialDelay);
    //   Running = true;
    // }

    // FireTimer.IfElapsed(
    //   () => BeginFiring()
    // );

    SweepTimer.UnlessElapsed(
      () => FireBeam()
    );

    LineRenderer.enabled = !SweepTimer.Elapsed();
  }

  public void BeginFiring() {
    SweepTimer.Reset();
  }

  void FireBeam() {
    var heading = LaserBeam.Fire(
      BeamAngle()
    );

    DrawBeam(heading);
  }

  void DrawBeam( Vector3 heading ) {
    LineRenderer.SetPosition( 0, transform.position );
    LineRenderer.SetPosition( 1, transform.position + heading );
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
