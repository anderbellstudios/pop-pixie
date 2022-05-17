using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWaitingBeforeThrow : MonoBehaviour {

  public BulletData BulletData;
  public float Speed;
  public FollowsPlayer FollowsPlayer;
  public Collider2D Collider;
  public TrailRenderer TrailRenderer;
  public Rigidbody2D Rigidbody;
  public bool Waiting = true;

  void Update() {
    if (!StateManager.Playing)
      return;

    if (Waiting && !WrappedInput.GetButton("Fire")) {
      Waiting = false;
      FollowsPlayer.enabled = false;
      Collider.enabled = true;
      TrailRenderer.emitting = true;
      Rigidbody.velocity = Speed * BulletData.DirectionManager.Direction;
    }
  }

}
