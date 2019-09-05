using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : AFireable {

  public LaserBeam LaserBeam;
  public LineRenderer LineRenderer;
  public float SweepDuration;
  public float InitialAngle, FinalAngle;
  public bool Firing;

  private IntervalTimer SweepTimer;

  void Start() {
    SweepTimer = new IntervalTimer() {
      Interval = SweepDuration
    };
  }

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( SweepTimer.Elapsed() )
      Firing = false;

    if ( Firing )
      FireBeam();

    LineRenderer.enabled = Firing;
  }

  public override void BeginFiring() {
    Firing = true;
    SweepTimer.Reset();
  }

  public override void StopFiring() {
    Firing = false;
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
