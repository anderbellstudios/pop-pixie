using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingRoom5Events : MonoBehaviour {
  public CameraPan CameraPan;
  public FollowsPlayer CameraFollowsPlayer;

  void Start() {
    StateManager.SetState( State.Dialogue );
    Invoke("StartCameraPan", 1f);
  }

  void StartCameraPan() {
    CameraPan.Perform(this, "AfterCameraPan");
  }

  public void AfterCameraPan() {
    StateManager.SetState( State.Playing );
    CameraFollowsPlayer.enabled = true;
  }
}
