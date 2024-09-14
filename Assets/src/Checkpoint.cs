using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour {
  public string Id;
  public SpriteRenderer DebugSpriteRenderer;

  private bool Resumed = false;

  void Awake() {
    if (Id == "") {
      throw new System.Exception("Id cannot be empty");
    }

    DebugSpriteRenderer.enabled = Debug.isDebugBuild;

    if (CheckpointData.LastCheckpointId == Id) {
      ResumeFromHere();
    }
  }

  void Start() {
    if (Resumed) {
      NotAnalytics.Current.Hit("resume-checkpoint", Id);
    }
  }

  public void Activate() {
    CheckpointData.LastCheckpointId = Id;
    CheckpointData.SaveGameData();

    if (DebugSpriteRenderer) {
      Color color = DebugSpriteRenderer.color;
      color.a = 0.8f;
      DebugSpriteRenderer.color = color;
    }
  }

  private void ResumeFromHere() {
    CheckpointData.LoadGameData();
    TeleportPlayerToHere();
    Resumed = true;
  }

  private void TeleportPlayerToHere() {
    // Cannot depend on PlayerGameObject in Awake 
    Transform player = GameObject.Find("Pixie").transform;
    Transform camera = Camera.main.transform;
    Vector3 offset = camera.position - player.position;
    player.position = transform.position;
    camera.position = transform.position + offset;
  }
}
