using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHintPrompt : MonoBehaviour {
  public float SpeedThreshold;
  public float TimeBeforeShow;

  private Stopwatch Stopwatch;

  void Start() {
    Stopwatch = new Stopwatch.PlayingTime();

    InGamePrompt.Current.RegisterSource(
      InGamePrompt.Priority.TutorialMove,
      () => Stopwatch.Time() > TimeBeforeShow ? HintText() : null
    );
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    if (PlayerGameObject.EstimatedVelocity.magnitude >= SpeedThreshold)
      Stopwatch.Reset();
  }

  string HintText() => InputMode.IsJoystick()
    ? "Use [Move Stick] to move"
    : "Use [Move Up][Move Left][Move Down][Move Right] to move";
}
