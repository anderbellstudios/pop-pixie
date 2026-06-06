using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHintPrompt : MonoBehaviour {
  public float TimeBeforeShow;

  void Start() {
    InGamePrompt.Current.RegisterSource(
      InGamePrompt.Priority.TutorialMove,
      () => PlayingTime.time - PlayerGameObject.LastMovedAt > TimeBeforeShow ? HintText() : null
    );
  }

  void Update() {
    if (!StateManager.Playing)
      return;
  }

  string HintText() =>
    InputMode.IsJoystick()
      ? "Use [Move Stick] to move"
      : "Use [Move Up][Move Left][Move Down][Move Right] to move";
}
