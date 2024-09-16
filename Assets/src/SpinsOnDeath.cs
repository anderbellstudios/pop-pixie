using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinsOnDeath : MonoBehaviour {
  public bool SingletonInstance = true;
  public static SpinsOnDeath Current;

  public float ZoomSpeed;
  public float SpinSpeed;
  public Camera Cam;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    this.enabled = false;
  }

  public static void Begin() {
    Current.enabled = true;
  }

  void Update() {
    Cam.orthographicSize -= ZoomSpeed * Time.deltaTime;
    gameObject.transform.Rotate(0, 0, SpinSpeed * Time.deltaTime);
  }
}
