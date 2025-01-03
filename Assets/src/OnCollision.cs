using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour {
  public bool StandardCollisions = true;
  public bool TriggerCollisions = false;
  public UnityEvent OnCollide;

  public Collider2D LastCollider { get; private set; }
  public bool WasStay { get; private set; }

  void OnTriggerEnter2D(Collider2D collider) {
    if (TriggerCollisions) {
      LastCollider = collider;
      WasStay = false;
      OnCollide.Invoke();
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (StandardCollisions) {
      LastCollider = collision.collider;
      WasStay = false;
      OnCollide.Invoke();
    }
  }

  void OnCollisionStay2D(Collision2D collision) {
    if (StandardCollisions) {
      LastCollider = collision.collider;
      WasStay = true;
      OnCollide.Invoke();
    }
  }
}
