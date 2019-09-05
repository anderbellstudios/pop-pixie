using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3MonologuePhase : APhase {

  public CameraPan Pan1;
  public CameraPan Pan2;
  public float DelayBeforePan1;
  public float DelayBeforePan2;

	public override void LocalBegin () {
    StateManager.SetState( State.Cutscene );
    Invoke("StartPan1", DelayBeforePan1);
  }

  void StartPan1() {
    Pan1.Perform(this, "Pan1Finished");
  }

  void Pan1Finished() {
    Invoke("StartPan2", DelayBeforePan2);
  }

  void StartPan2() {
    Pan2.Perform(this, "Pan2Finished");
  }

  void Pan2Finished() {
    StateManager.SetState( State.Playing );
    PhaseFinished();
  }

}
