using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {

  public float Speed;
  public float Amplitude;

  void Update() {
    transform.localPosition = new Vector3(
      0,
      Amplitude * Mathf.Sin(Speed * Time.time),
      0
    );
  }

}
