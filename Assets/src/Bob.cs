using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {
  public static bool TestMode = false;

  public Vector3 Direction = new Vector3(0, 1, 0);
  public float Speed;
  public float Amplitude;
  public bool RandomInitialPhase;

  Vector3 InitialPosition;
  float PhaseOffset = 0;

  void Start() {
    if (RandomInitialPhase) {
      PhaseOffset = Random.Range(0, 360);
    }

    InitialPosition = transform.localPosition;
  }

  void Update() {
    if (TestMode)
      return;
    transform.localPosition =
      InitialPosition +
      Direction *
      Amplitude *
      Mathf.Sin(Speed * Time.time + PhaseOffset);
  }
}
