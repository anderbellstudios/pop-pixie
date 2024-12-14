using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour {
  public bool StandardCollisions = true;
  public bool TriggerCollisions = false;
  public UnityEvent OnCollide;

  public Collider2D LastCollider { get; private set; }

  void OnTriggerEnter2D(Collider2D collider) {
    if (TriggerCollisions) {
      LastCollider = collider;
      OnCollide.Invoke();
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (StandardCollisions) {
      LastCollider = collision.collider;
      OnCollide.Invoke();
    }
  }
}
