using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

  public Rigidbody2D rb;
  public Vector2 Movement;

  void FixedUpdate() {
    if ( StateManager.Is( State.Playing ) ) {
      rb.MovePosition(rb.position + Movement * Time.fixedDeltaTime);
    }

    Movement = Vector2.zero;
  }

}
