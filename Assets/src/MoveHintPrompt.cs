using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHintPrompt : MonoBehaviour {
  public float SpeedThreshold;
  public float TimeBeforeShow;

  private float LastMoved;

  void Start() {
    InGamePrompt.Current.RegisterSource(100, () =>
      PlayingTime.time - LastMoved > TimeBeforeShow
      ? HintText()
      : null
    );

    LastMoved = PlayingTime.time;
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    if (PlayerGameObject.EstimatedVelocity.magnitude >= SpeedThreshold)
      LastMoved = PlayingTime.time;
  }

  string HintText() => InputMode.IsJoystick()
    ? "Use [Move Stick] to move"
    : "Use [Move Up][Move Left][Move Down][Move Right] to move";
}
