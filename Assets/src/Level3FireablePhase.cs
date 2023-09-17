using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level3FireablePhase : APhase {

  public FireableScheduler FireableScheduler;
  public List<HitPoints> FireableHitPoints;

  public override void LocalBegin() {
    FireableScheduler.enabled = true;
  }

  public override void WhilePhaseRunning() {
    if (FireableScheduler.Fireables.Count == 0)
      PhaseFinished();
  }

  public override void AfterFinished() {
    FireableScheduler.enabled = false;
  }

  public override float ProgressBarAllotment() {
    return 1f;
  }

  public float TotalMaxHP() {
    return FireableHitPoints.Sum(hp => hp.Maximum);
  }

  public override float ProgressBarValue() {
    return FireableHitPoints.Sum(hp => hp.Current) / TotalMaxHP();
  }

}
