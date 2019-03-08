using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMotion : MonoBehaviour {

  public float Amplitude;
  public float CorrectingFactor;
  public float MinInterval, MaxInterval;
  public Rigidbody2D Body;

  private Vector3 InitialPosition;
  private DateTime LastWobbled;
  private float WobbleInterval;

  void Start () {
    InitialPosition = transform.position;
  }

  // Update is called once per frame
  void Update () {
    if ( ShouldWobble() ) {
      UpdateWobbleInterval();

      transform.Translate( new Vector3(
        RandomNoise(),
        RandomNoise(),
        0
      ));
    }

    Body.velocity = ( 
      CorrectingFactor * ( InitialPosition - transform.position )
    );
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
