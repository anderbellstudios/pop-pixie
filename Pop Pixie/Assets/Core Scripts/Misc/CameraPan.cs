using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraPan : MonoBehaviour {
  public Camera Camera;
  public Camera DestinationCamera;
  public float Duration;
  public bool PauseGameplay, FollowPlayerAfterPan;

  [SerializeField] public UnityEvent OnFinish;

  private Vector3 InitialPosition;
  private float InitialSize;
  private IntervalTimer Timer;

  void Awake() {
    Timer = new IntervalTimer() {
      Interval = Duration
    };
  }

  public void Perform() {
    InitialPosition = Camera.transform.position;
    InitialSize = Camera.GetComponent<Camera>().orthographicSize;

    if (PauseGameplay)
      StateManager.SetState( State.Cutscene );

    Timer.Reset();
  }

  void Update() {
    Timer.UnlessElapsed(() => {
      Camera.transform.position = InterpolatedPosition();
      Camera.GetComponent<Camera>().orthographicSize = InterpolatedSize();
    });

    Timer.IfElapsed(() => {
      Finished();
      Timer.Stop();
    });
  }

  void Finished() {
    if (PauseGameplay)
      StateManager.SetState( State.Playing );

    Camera.GetComponent<FollowsPlayer>().enabled = FollowPlayerAfterPan;

    OnFinish.Invoke();
  }

  Vector3 InterpolatedPosition() {
    return Vector3.Lerp(
      InitialPosition,
      DestinationCamera.transform.position,
      Timer.Progress()
    );
  }

  float InterpolatedSize() {
    return Mathf.Lerp(
      InitialSize,
      DestinationCamera.GetComponent<Camera>().orthographicSize,
      Timer.Progress()
    );
  }
}
