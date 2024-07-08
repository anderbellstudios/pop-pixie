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
  public GrenadePredictedPosition GrenadePredictedPosition;
  public GrenadeExplodesAfterTime GrenadeExplodesAfterTime;
  public bool Waiting = true;

  void Update() {
    if (!StateManager.Playing || !Waiting)
      return;

    if (!WrappedInput.GetButton("Fire")) {
      StopWaiting();
      return;
    }

    GrenadePredictedPosition.UpdatePosition(
      direction: BulletData.GetDirection(),
      speed: Speed,
      radius: GrenadeExplodesAfterTime.Radius,
      explodeTime: GrenadeExplodesAfterTime.ExplodeTime
    );
  }

  void StopWaiting() {
    GrenadePredictedPosition.Hide();
    Waiting = false;
    FollowsPlayer.enabled = false;
    Collider.enabled = true;
    TrailRenderer.emitting = true;
    Rigidbody.velocity = Speed * BulletData.GetDirection();
  }
}
