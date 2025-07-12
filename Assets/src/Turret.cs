using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
  public Transform SpriteTransform;
  public FireBullets FireBullets;
  public float FireDuration,
    CoolDownDuration;
  public float AimSpeed = -1f;

  private bool CoolingDown = false;
  private Vector3 VirtualPlayerDirection = Vector3.up;

  void Update() {
    if (!StateManager.Playing)
      return;

    if (CanSeePlayer()) {
      if (AimSpeed > 0) {
        VirtualPlayerDirection = Vector3.MoveTowards(
          VirtualPlayerDirection,
          DirectionToPlayer,
          AimSpeed * Time.deltaTime
        );
      } else {
        VirtualPlayerDirection = DirectionToPlayer;
      }

      if (!FireBullets.Firing && !CoolingDown) {
        StartFiring();
      }
    }

    SpriteTransform.rotation = Quaternion.Euler(
      0,
      0,
      Mathf.Atan2(VirtualPlayerDirection.y, VirtualPlayerDirection.x) * Mathf.Rad2Deg
    );
  }

  void OnDisable() {
    FireBullets.StopFiring();
  }

  private void StartFiring() {
    FireBullets.BeginFiring(getTarget: () =>
      transform.position + DistanceToPlayer * VirtualPlayerDirection
    );

    AsyncTimer.PlayingTime.SetTimeout(StopFiring, FireDuration, bindToBehaviour: this);
  }

  private void StopFiring() {
    FireBullets.StopFiring();
    CoolingDown = true;

    AsyncTimer.PlayingTime.SetTimeout(
      () => CoolingDown = false,
      CoolDownDuration,
      bindToBehaviour: this
    );
  }

  private Vector3 VectorToPlayer => PlayerGameObject.Position - transform.position;
  private Vector3 DirectionToPlayer => VectorToPlayer.normalized;
  private float DistanceToPlayer => VectorToPlayer.magnitude;

  private bool CanSeePlayer() =>
    !Physics2D.Raycast(
      transform.position,
      DirectionToPlayer,
      DistanceToPlayer,
      CollisionMask.OpaqueMask
    );
}
