using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRingPull : MonoBehaviour {

  public Rigidbody2D rb;
  public float InitialSpeed;

  void Start() {
    rb.velocity = InitialSpeed * Random.insideUnitCircle.normalized;
  }

}
