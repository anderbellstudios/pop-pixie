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

  private Stopwatch Stopwatch;

  public override void LocalBegin() {
    Stopwatch = UsePlayingTime
      ? new Stopwatch.PlayingTime()
      : new Stopwatch.BaseTime();

#if UNITY_EDITOR
    Debug.Assert(!(UsePlayingTime && PauseGameplay), "DelayPhase cannot use playing time AND pause gameplay");
#endif

    if (PauseGameplay)
      StateManager.AddState(State.NotPlaying);

    DialogueMusicFadeBehaviour.ApplyEnterBehaviour();
    OnBegin.Invoke();
  }

  public override void WhilePhaseRunning() {
    float progress = Stopwatch.Progress(Delay);

    HUDBar?.SetProgress(progress);

    if (progress >= 1f) {
      if (PauseGameplay)
        StateManager.RemoveState(State.NotPlaying);

      DialogueMusicFadeBehaviour.ApplyExitBehaviour();
      OnFinish.Invoke();
      PhaseFinished();
    };
  }
}
