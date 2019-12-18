using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {

  public float Speed;
  public float Amplitude;
  public bool RandomInitialPhase;

  float PhaseOffset = 0;

  void Start() {
    if ( RandomInitialPhase ) {
      PhaseOffset = Random.Range(0, 360);
    }
  }

  void Update() {
    transform.localPosition = new Vector3(
      0,
      Amplitude * Mathf.Sin(Speed * Time.time + PhaseOffset),
      0
    );
  }

}
