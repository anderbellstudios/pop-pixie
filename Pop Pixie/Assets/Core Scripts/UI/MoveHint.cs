using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHint : MonoBehaviour {
  public GameObject KeyboardAndMouseHint, JoystickHint;
  private LowPriorityBehaviour LowPriorityBehaviour;

  void OnEnable() {
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  void Update() {
    LowPriorityBehaviour.EveryNFrames(10, () => {
      bool isJoystick = InputMode.IsJoystick();
      KeyboardAndMouseHint.SetActive(!isJoystick);
      JoystickHint.SetActive(isJoystick);
    });
  }
}
