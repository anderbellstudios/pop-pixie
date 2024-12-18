using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraPan : MonoBehaviour {
  public Camera DestinationCamera;
  public float Duration;
  public bool PauseGameplay, FollowPlayerAfterPan;

  public UnityEvent OnFinish;

  public void Perform() {
    if (PauseGameplay)
      StateManager.AddState(State.NotPlaying);

    CameraManager.Current.AnimateTo(
      CameraState.FromCamera(DestinationCamera, FollowPlayerAfterPan),
      Duration,
      Finished
    );
  }

  private void Finished() {
    if (PauseGameplay)
      StateManager.RemoveState(State.NotPlaying);

    OnFinish.Invoke();
  }
}
