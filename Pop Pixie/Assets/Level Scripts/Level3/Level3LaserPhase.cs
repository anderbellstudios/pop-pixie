using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3LaserPhase : APhase {

  public LaserScheduler LaserScheduler;

	public override void LocalBegin () {
    LaserScheduler.enabled = true;
  }

  public override void WhilePhaseRunning() {
    if ( LaserScheduler.Lasers.Count == 0 )
      PhaseFinished();
  }

}
