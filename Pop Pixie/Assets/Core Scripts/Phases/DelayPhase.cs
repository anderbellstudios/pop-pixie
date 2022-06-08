using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPhase : APhase {

  public bool PauseGameplay;
  public bool UsePlayingTime = false;
  public float Delay;
  public HUDBar HUDBar;

  private IntervalTimer Timer;

	public override void LocalBegin () {
    Timer = new IntervalTimer() {
      TimeClass = UsePlayingTime ? "PlayingTime" : "Time",
      Interval = Delay
    };

#if UNITY_EDITOR
    Debug.Assert(!(UsePlayingTime && PauseGameplay), "DelayPhase cannot use playing time AND pause gameplay");
#endif

    if (PauseGameplay)
      StateManager.AddState(State.NotPlaying);

    Timer.Reset();
  }

  public override void WhilePhaseRunning() {
    if (HUDBar != null)
      HUDBar.Progress = Timer.Progress();

    Timer.IfElapsed(() => {
      if (PauseGameplay)
        StateManager.RemoveState(State.NotPlaying);

      PhaseFinished();
    });
  }

}
