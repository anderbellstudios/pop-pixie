using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour {

  public float Speed;
  public bool RandomInitialRotation;

  void Start() {
    if (RandomInitialRotation) {
      gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
    }
  }

  void OnDisable() {
    gameObject.transform.rotation = Quaternion.identity;
  }

  void Update() {
    gameObject.transform.Rotate(0, Speed * Time.deltaTime, 0);
  }

}
