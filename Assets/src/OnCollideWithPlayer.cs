using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollideWithPlayer : MonoBehaviour {
  public UnityEvent OnCollide;

  void OnTriggerEnter2D(Collider2D other) {
    if ( other.tag == "Player" ) {
      OnCollide.Invoke();
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if ( collision.gameObject.tag == "Player" ) {
      OnCollide.Invoke();
    }
  }
}
