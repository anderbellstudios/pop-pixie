using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {
  public Camera Camera;
  public Camera DestinationCamera;
  public float Duration;

  private DateTime StartedAt;
  private Vector3 InitialPosition;
  private float InitialSize;

  public void Perform(MonoBehaviour target, string callback) {
    InitialPosition = Camera.transform.position;
    InitialSize = Camera.GetComponent<Camera>().orthographicSize;

    StartedAt = DateTime.Now;
    target.Invoke(callback, Duration);
  }

  void FixedUpdate() {
    if ( InProgress() ) {
      Camera.transform.position = InterpolatedPosition();
      Camera.GetComponent<Camera>().orthographicSize = InterpolatedSize();
    }
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
