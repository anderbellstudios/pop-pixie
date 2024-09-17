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
  private Stopwatch Stopwatch;
  private bool Running = false;

  public void Perform() {
    InitialPosition = Camera.transform.position;
    InitialSize = Camera.orthographicSize;

    if (PauseGameplay)
      StateManager.AddState(State.NotPlaying);

    Stopwatch = new Stopwatch.BaseTime();
    Running = true;
  }

  void Update() {
    if (!Running)
      return;

    float progress = Stopwatch.Progress(Duration);
    Camera.transform.position = InterpolatedPosition(progress);
    Camera.GetComponent<Camera>().orthographicSize = InterpolatedSize(progress);

    if (progress >= 1f) {
      Finished();
    }
  }

  void Finished() {
    Running = false;

    if (PauseGameplay)
      StateManager.RemoveState(State.NotPlaying);

    Camera.transform.position = DestinationCamera.transform.position;
    Camera.orthographicSize = DestinationCamera.orthographicSize;
    Camera.GetComponent<FollowsPlayer>().enabled = FollowPlayerAfterPan;

    OnFinish.Invoke();
  }

  Vector3 InterpolatedPosition(float progress) => Vector3.Lerp(
    InitialPosition,
    DestinationCamera.transform.position,
    progress
  );

  float InterpolatedSize(float progress) => Mathf.Lerp(
    InitialSize,
    DestinationCamera.orthographicSize,
    progress
  );
}
