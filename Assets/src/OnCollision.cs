using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour {
  public UnityEvent OnCollide;

  public Collider2D LastCollider { get; private set; }

  void OnCollisionEnter2D(Collision2D collision) {
    LastCollider = collision.collider;
    OnCollide.Invoke();
  }
}
