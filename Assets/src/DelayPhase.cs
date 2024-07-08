using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayPhase : APhase {
  public bool PauseGameplay;
  public bool UsePlayingTime = false;
  public float Delay;
  public HUDBar HUDBar;
  public DialogueMusicFadeBehaviour DialogueMusicFadeBehaviour = new DialogueMusicFadeBehaviour(
    DialogueMusicFadeBehaviour.BehaviourType.Ignore,
    DialogueMusicFadeBehaviour.BehaviourType.Ignore
  );
  public UnityEvent OnBegin, OnFinish;

  private IntervalTimer Timer;

  public override void LocalBegin() {
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

    DialogueMusicFadeBehaviour.ApplyEnterBehaviour();
    OnBegin.Invoke();
  }

  public override void WhilePhaseRunning() {
    if (HUDBar != null)
      HUDBar.Progress = Timer.Progress();

    Timer.IfElapsed(() => {
      if (PauseGameplay)
        StateManager.RemoveState(State.NotPlaying);

      DialogueMusicFadeBehaviour.ApplyExitBehaviour();
      OnFinish.Invoke();
      PhaseFinished();
    });
  }
}
