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

  private DateTime StartedAt;
  private Vector3 InitialPosition;
  private float InitialSize;

  public void Perform() {
    InitialPosition = Camera.transform.position;
    InitialSize = Camera.GetComponent<Camera>().orthographicSize;

    if (PauseGameplay)
      StateManager.SetState( State.Cutscene );

    StartedAt = DateTime.Now;
    Invoke("Finished", Duration);
  }

  void FixedUpdate() {
    if ( InProgress() ) {
      Camera.transform.position = InterpolatedPosition();
      Camera.GetComponent<Camera>().orthographicSize = InterpolatedSize();
    }
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
      Progress()
    );
  }

  float InterpolatedSize() {
    return Mathf.Lerp(
      InitialSize,
      DestinationCamera.GetComponent<Camera>().orthographicSize,
      Progress()
    );
  }

  float Progress() {
    var since = (float) DateTime.Now.Subtract( StartedAt ).TotalSeconds;

    return Mathf.Clamp(
      since / Duration,
      0.0f,
      1.0f
    );
  }
  
  bool InProgress() {
    if ( StartedAt == null )
      return false;

    return Progress() < 1.0f;
  }
}
