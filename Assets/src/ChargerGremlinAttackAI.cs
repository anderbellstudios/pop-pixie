using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerGremlinAttackAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  public SpriteRenderer DirectionIndicator;
  public float PrepareSpeed,
    RecoverSpeed;
  public float WiggleAmplitude;
  public float PrepareWiggleSpeed,
    ChargeWiggleSpeed,
    RecoverWiggleSpeed;
  public float PrepareDuration,
    MaxChargeDuration,
    RecoverDuration;
  public float Damage;

  private enum StateType {
    Preparing,
    Charging,
    Recovering,
  };

  private StateType State = StateType.Preparing;
  private Transform WiggleTransform;
  private EnemyHitPointEvents HitPointEvents;
  private bool PreviousSlowOnDamage;
  private float WigglePhase;
  private Vector2 Direction;
  private Vector2 ChargeStartPosition;
  private Stopwatch Stopwatch;

  void Start() {
    Helper.OnAnyCollision(
      (collider) => {
        if (State != StateType.Charging)
          return;

        // Do not stop charging when hit by bullet
        if (IsBullet(collider.gameObject.layer))
          return;

        // Do not stop charging when touching a wall
        if (Helper.CollisionWasStay && !Helper.CollisionWasPlayer)
          return;

        HitPoints hitPoints = collider.gameObject.GetComponent<HitPoints>();

        if (hitPoints) {
          bool isCounterAttack = hitPoints.Damage(Damage, true);

          if (isCounterAttack) {
            Helper.KillSelf();
          }
        }

        BeginRecovering();
      }
    );

    WiggleTransform = Helper.Transform.Find("Sprite");

    HitPointEvents = Helper.Transform.Find("Hit Point Events").GetComponent<EnemyHitPointEvents>();
  }

  protected override void OnActivate() {
    BeginPreparing();
    WigglePhase = 0f;
  }

  protected override void OnDeactivate() {
    SetWiggleAngle(0f);
    HideDirectionIndicator();
    ResetSlowOnDamage();
  }

  protected override bool ShouldAvoidInterruption() => State != StateType.Preparing;

  protected override void WhileActive() {
    switch (State) {
      case StateType.Preparing:
        WhilePreparing();
        break;

      case StateType.Charging:
        WhileCharging();
        break;

      case StateType.Recovering:
        WhileRecovering();
        break;
    }
  }

  private void BeginPreparing() {
    SetState(StateType.Preparing);
    Helper.SetTimeout(BeginCharging, PrepareDuration);
    ShowDirectionIndicator();
  }

  private void BeginCharging() {
    SetState(StateType.Charging);
    DisableSlowOnDamage();
    HideDirectionIndicator();

    if (MaxChargeDuration != Mathf.Infinity) {
      Helper.SetTimeout(
        () => {
          if (State == StateType.Charging) {
            GiveUpCharging();
          }
        },
        MaxChargeDuration
      );
    }
  }

  private void BeginRecovering() {
    SetState(StateType.Recovering);
    Helper.SetTimeout(FinishRecovering, RecoverDuration);
  }

  private void GiveUpCharging() {
    OnFinish();
  }

  private void FinishRecovering() {
    OnFinish();
  }

  private void WhilePreparing() {
    Direction = Helper.DirectionToPlayer;
    ChargeStartPosition = Helper.Position;
    float progress = Stopwatch.Progress(PrepareDuration);
    Helper.MoveWithVelocity(-1f * (1f - progress) * PrepareSpeed * Direction);
    Wiggle(PrepareWiggleSpeed * progress);
    UpdateDirectionIndicator(progress);
  }

  private void WhileCharging() {
    Helper.MoveWithVelocity(Speed * Direction);
    Wiggle(ChargeWiggleSpeed);
  }

  private void WhileRecovering() {
    float progress = 1f - Stopwatch.Progress(RecoverDuration);
    float recoilProgress = Mathf.Pow(progress, 10f);
    Helper.MoveWithVelocity(-1f * recoilProgress * RecoverSpeed * Direction);
    Wiggle(RecoverWiggleSpeed * progress);
  }

  private void SetState(StateType state) {
    Stopwatch = new Stopwatch.PlayingTime();
    State = state;
  }

  private void Wiggle(float speed) {
    WigglePhase += speed * Time.deltaTime;
    SetWiggleAngle(Mathf.Sin(WigglePhase) * WiggleAmplitude);
  }

  private void SetWiggleAngle(float angle) {
    WiggleTransform.rotation = Quaternion.Euler(0, 0, angle);
  }

  private bool IsBullet(int layer) => ((1 << layer) & CollisionMask.BulletMask) != 0;

  private void DisableSlowOnDamage() {
    PreviousSlowOnDamage = HitPointEvents.SlowOnDamage;
    HitPointEvents.SlowOnDamage = false;
  }

  private void ResetSlowOnDamage() {
    HitPointEvents.SlowOnDamage = PreviousSlowOnDamage;
  }

  private void ShowDirectionIndicator() {
    DirectionIndicator.gameObject.SetActive(true);
    UpdateDirectionIndicator(0f);
  }

  private void UpdateDirectionIndicator(float progress) {
    float maxDistance = Speed * MaxChargeDuration;

    Vector2 chargeVector = Vector2.ClampMagnitude(Helper.VectorToPlayer, maxDistance) * progress;

    DirectionIndicator.transform.localRotation = Quaternion.FromToRotation(
      Vector3.right,
      chargeVector.normalized
    );

    DirectionIndicator.transform.position = ChargeStartPosition + chargeVector / 2f;

    DirectionIndicator.size = new Vector2(chargeVector.magnitude, DirectionIndicator.size.y);
  }

  private void HideDirectionIndicator() {
    DirectionIndicator.gameObject.SetActive(false);
  }
}
