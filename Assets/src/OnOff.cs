using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnOff : MonoBehaviour {
  public bool StartOn, RestartOnEnable;
  public float OnDuration, OffDuration;
  public UnityEvent OnTurnOn, OnTurnOff;

  private bool On;
  private float Offset;

  void Start() {
    Restart();
  }

  void OnEnable() {
    if (RestartOnEnable) {
      Restart();
    }
  }

  private void Restart() {
    SetOn(StartOn);
    Offset = (StartOn ? 0 : OnDuration) - PlayingTime.time;
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    float time = (PlayingTime.time + Offset) % (OnDuration + OffDuration);
    bool isOn = time < OnDuration;

    if (On != isOn) {
      SetOn(isOn);
    }
  }

  private void SetOn(bool isOn) {
    On = isOn;
    (isOn ? OnTurnOn : OnTurnOff).Invoke();
  }
}
