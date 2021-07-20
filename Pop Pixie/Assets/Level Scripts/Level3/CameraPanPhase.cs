using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanPhase : APhase {

  public CameraPan Pan;
  public bool Cutscene;
  public float DelayBeforePan;

	public override void LocalBegin () {
    if ( Cutscene ) StateManager.SetState( State.Cutscene );
    Invoke("StartPan", DelayBeforePan);
  }

  void StartPan() {
    // Pan.Perform(this, "PanFinished");
    PanFinished();
  }

  void PanFinished() {
    if ( Cutscene ) StateManager.SetState( State.Playing );
    PhaseFinished();
  }

}
