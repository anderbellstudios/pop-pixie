using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : AFireable {

  public LaserBeam LaserBeam;
  public LineRenderer LineRenderer;
  public float SweepDuration;
  public float InitialAngle, FinalAngle;
  public bool Firing;

  [SerializeField] public UnityEvent OnStop;
  [SerializeField] public UnityEvent OnFinished;

  private IntervalTimer SweepTimer;

  void Awake() {
    SweepTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = SweepDuration
    };
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    if (SweepTimer.Elapsed()) {
      StopFiring();
      OnFinished.Invoke();
    }

    if (Firing)
      FireBeam();

    LineRenderer.enabled = Firing;
  }

  public override void BeginFiring() {
    Firing = true;
    SweepTimer.Reset();
  }

  public override void StopFiring() {
    Firing = false;
    OnStop.Invoke();
  }

  void FireBeam() {
    var heading = LaserBeam.Fire(
      BeamAngle()
    );

    DrawBeam(heading);
  }

  void DrawBeam(Vector3 heading) {
    LineRenderer.SetPosition(0, transform.position);
    LineRenderer.SetPosition(1, transform.position + heading);
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
