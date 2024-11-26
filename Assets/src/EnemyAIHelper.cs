using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIHelper {
  public GameObject GameObject;

  private AEnemyAI2 AI;
  private HitPoints HitPoints;
  private MovementManager MovementManager;
  private NavigateToPoint NavigateToPoint;
  private CapsuleCollider2D CapsuleCollider2D;
  private OnCollision OnCollisionComponent;
  private bool MovementAllowed;
  private LayerMask LineOfSightMask, LineOfMovementMask;
  private List<AsyncTimer.EnqueuedEvent> Timers = new();

  public EnemyAIHelper(AEnemyAI2 ai, GameObject gameObject, bool movementAllowed) {
    AI = ai;
    GameObject = gameObject;
    HitPoints = gameObject.GetComponent<HitPoints>();
    MovementManager = gameObject.GetComponent<MovementManager>();
    NavigateToPoint = gameObject.GetComponent<NavigateToPoint>();
    CapsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
    OnCollisionComponent = gameObject.GetComponent<OnCollision>();
    MovementAllowed = movementAllowed;

    // Can see through anything except "Default"
    LineOfSightMask = LayerMask.GetMask("Default");
  }

  public Transform Transform => GameObject.transform;
  public Vector3 Position => Transform.position;

  public GameObject Player => PlayerGameObject.Current;
  public Transform PlayerTransform => Player.transform;
  public Vector3 PlayerPosition => PlayerTransform.position;
  public HitPoints PlayerHitPoints => Player.GetComponent<HitPoints>();

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

  public void OnAnyCollision(Action<Collider2D> handler) {
    OnCollisionComponent.OnCollide.AddListener(() => {
      if (AI.IsActive) {
        handler(OnCollisionComponent.LastCollider);
      }
    });
  }

  public void OnPlayerCollision(Action handler) {
    OnAnyCollision(collider => {
      if (collider.tag == "Player") {
        handler();
      }
    });
  }

  public void OnDamage(Action handler) {
    HitPoints.OnDecrease.AddListener(_ => {
      if (AI.IsActive) {
        handler();
      }
    });
  }

  public bool DamagePlayer(float damage, bool canBeCounterAttacked) {
    return PlayerHitPoints.Damage(damage, canBeCounterAttacked);
  }

  public void KillSelf() {
    HitPoints.Damage(Mathf.Infinity);
  }

  public List<GameObject> OtherEnemies() => GameObject
    .FindGameObjectsWithTag("Enemy")
    .Where(enemy => enemy != GameObject)
    .ToList();

  public AsyncTimer.EnqueuedEvent SetTimeout(System.Action callback, float timeout) {
    AsyncTimer.EnqueuedEvent timer = AsyncTimer.PlayingTime.SetTimeout(callback, timeout, GameObject);
    Timers.Add(timer);
    return timer;
  }

  public AsyncTimer.EnqueuedEvent SetInterval(System.Action callback, float interval) {
    AsyncTimer.EnqueuedEvent timer = AsyncTimer.PlayingTime.SetInterval(callback, interval, GameObject);
    Timers.Add(timer);
    return timer;
  }

  public void ClearTimeout(AsyncTimer.EnqueuedEvent timer) {
    AsyncTimer.PlayingTime.ClearTimeout(timer);
    Timers.Remove(timer);
  }

  private void ClearTimers() {
    Timers.ForEach(AsyncTimer.PlayingTime.ClearTimeout);
    Timers.Clear();
  }

  // Called by AEnemyAI2
  public void Deactivate() {
    ClearTimers();
  }

  private void CheckMovementAllowed() {
#if UNITY_EDITOR
    Debug.Assert(MovementAllowed, "Only AIs inheriting from AMovementEnemyAI are allowed to perform movement.");
#endif
  }
}
