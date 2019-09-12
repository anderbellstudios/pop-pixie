// http://answers.unity.com/answers/33169/view.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingOnPause : MonoBehaviour {

  public Rigidbody2D rb;

  public bool WasPlayingLastFrame;

  public Vector2 saveVelocity;
  public float saveAngularVelocity;

  void Start() {
    WasPlayingLastFrame = true;
    rb = GetComponent<Rigidbody2D>();
  }

  void Update() {
    bool isPlaying = StateManager.Is( State.Playing );

    if ( ! isPlaying && WasPlayingLastFrame ) {
      saveVelocity = rb.velocity;
      saveAngularVelocity = rb.angularVelocity;

      rb.velocity = Vector2.zero;
      rb.angularVelocity = 0f;
    }

    if ( isPlaying && ! WasPlayingLastFrame ) {
      rb.velocity = saveVelocity;
      rb.angularVelocity = saveAngularVelocity;
      rb.WakeUp();
    }

    WasPlayingLastFrame = isPlaying;
  }

}
