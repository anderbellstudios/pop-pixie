using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3FireablePhase : APhase {

  public FireableScheduler FireableScheduler;

	public override void LocalBegin () {
    FireableScheduler.enabled = true;
  }

  public override void WhilePhaseRunning() {
    if ( FireableScheduler.Fireables.Count == 0 )
      PhaseFinished();
  }

}
