using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIHelper {
  public GameObject GameObject;

  private MovementManager MovementManager;
  private NavigateToPoint NavigateToPoint;
  private CapsuleCollider2D CapsuleCollider2D;
  private bool MovementAllowed;
  private LayerMask LineOfSightMask, LineOfMovementMask;

  public EnemyAIHelper(GameObject gameObject, bool movementAllowed) {
    GameObject = gameObject;
    MovementManager = gameObject.GetComponent<MovementManager>();
    NavigateToPoint = gameObject.GetComponent<NavigateToPoint>();
    CapsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
    MovementAllowed = movementAllowed;

    // Can see through anything except "Default"
    LineOfSightMask = LayerMask.GetMask("Default");
  }

  public Transform Transform => GameObject.transform;
  public Vector3 Position => Transform.position;

  public GameObject Player => PlayerGameObject.Current;
  public Transform PlayerTransform => Player.transform;
  public Vector3 PlayerPosition => PlayerTransform.position;

  public Vector3 VectorToPlayer => PlayerPosition - Position;
  public Vector3 DirectionToPlayer => VectorToPlayer.normalized;
  public float DistanceToPlayer => VectorToPlayer.magnitude;

  public void MoveWithVelocity(Vector2 velocity) {
    MoveWithDisplacement(velocity * Time.deltaTime);
  }

  public void MoveWithDisplacement(Vector2 displacement) {
    CheckMovementAllowed();
    MovementManager.Movement += displacement;
  }

  public void MoveTowardsPlayer(float speed) {
    MoveWithVelocity(DirectionToPlayer * speed);
  }

  public void EnableNavigateToPoint(float speed) {
    CheckMovementAllowed();
    NavigateToPoint.enabled = true;
    NavigateToPoint.Speed = speed;
  }

  public void DisableNavigateToPoint() {
    NavigateToPoint.enabled = false;
  }

  public void SetNavigateToPointDestination(Vector3 destination) {
    NavigateToPoint.DestinationPoint = destination;
  }

  public void NavigateToPlayer() {
    SetNavigateToPointDestination(PlayerPosition);
  }

  public bool CanSeePlayer() => !Physics2D.Raycast(
    Position,
    DirectionToPlayer,
    DistanceToPlayer,
    LineOfSightMask
  );

  public bool CanMoveToPlayer() => LineOfMovement.Check(
    start: Position,
    end: PlayerPosition,
    capsuleSize: CapsuleCollider2D.size
  );

  private void CheckMovementAllowed() {
#if UNITY_EDITOR
    Debug.Assert(MovementAllowed, "Only AIs inheriting from AMovementEnemyAI are allowed to perform movement.");
#endif
  }
}
