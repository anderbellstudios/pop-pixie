using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour {

  public float Speed;

  void Update() {
    gameObject.transform.Rotate(0, Speed * Time.deltaTime, 0);
  }

}
