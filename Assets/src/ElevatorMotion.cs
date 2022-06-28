using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMotion : MonoBehaviour {

  public float Amplitude;
  public float CorrectingFactor;
  public float MinInterval, MaxInterval;

  private Vector3 Displacement;
  private DateTime LastWobbled;
  private float WobbleInterval;

  // Update is called once per frame
  void Update () {
    if ( ShouldWobble() ) {
      UpdateWobbleInterval();

      Displace(new Vector3(
        RandomNoise(),
        RandomNoise(),
        0
      ));
    }

    Displace(CorrectingFactor * -1 * Displacement);
  }

  void Displace(Vector3 amount) {
    Displacement += amount;
    transform.Translate(amount);
  }

  bool ShouldWobble () {
    var since = DateTime.Now.Subtract( LastWobbled ).TotalSeconds;
    return since > WobbleInterval;
  }

  void UpdateWobbleInterval () {
    LastWobbled = DateTime.Now;

    WobbleInterval = MinInterval 
      + ( MaxInterval - MinInterval ) 
      * ( UnityEngine.Random.value );
  }

  float RandomNoise () {
    return Amplitude * ( UnityEngine.Random.value - 0.5f );
  }

}
