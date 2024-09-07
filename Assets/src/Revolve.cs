using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour {
  public static bool TestMode = false;

  public float Speed;
  public bool RandomInitialRotation;

  void Start() {
    if (TestMode) return;
    if (RandomInitialRotation) {
      gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
    }
  }

  void OnDisable() {
    if (TestMode) return;
    gameObject.transform.rotation = Quaternion.identity;
  }

  void Update() {
    if (TestMode) return;
    gameObject.transform.Rotate(0, Speed * Time.deltaTime, 0);
  }
}
