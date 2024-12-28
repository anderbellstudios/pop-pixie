using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerGremlinAttackAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  public float PrepareSpeed, RecoverSpeed;
  public float WiggleAmplitude;
  public float PrepareWiggleSpeed, ChargeWiggleSpeed, RecoverWiggleSpeed;
  public float PrepareDuration, MaxChargeDuration, RecoverDuration;
  public float Damage;

  private enum StateType { Preparing, Charging, Recovering };
  private StateType State = StateType.Preparing;
  private Transform WiggleTransform;
  private EnemyHitPointEvents HitPointEvents;
  private bool PreviousSlowOnDamage;
  private float WigglePhase;
  private Vector2 Direction;
  private Stopwatch Stopwatch;

  void Start() {
    Helper.OnAnyCollision((collider) => {
      if (State != StateType.Charging)
        return;

      if (IsBullet(collider.gameObject.layer))
        return;

      HitPoints hitPoints = collider.gameObject.GetComponent<HitPoints>();

      if (hitPoints) {
        bool isCounterAttack = hitPoints.Damage(Damage, true);

        if (isCounterAttack) {
          Helper.KillSelf();
        }
      }

      SetState(StateType.Recovering);

      Helper.SetTimeout(OnFinish, RecoverDuration);
    });

    WiggleTransform = Helper.Transform.Find("Sprite");

    HitPointEvents = Helper.Transform
      .Find("Hit Point Events")
      .GetComponent<EnemyHitPointEvents>();
  }

  protected override void OnActivate() {
    SetState(StateType.Preparing);

    Helper.SetTimeout(() => {
      SetState(StateType.Charging);
      DisableSlowOnDamage();
    }, PrepareDuration);

    if (MaxChargeDuration != Mathf.Infinity) {
      Helper.SetTimeout(() => {
        if (State == StateType.Charging) {
          OnFinish();
        }
      }, PrepareDuration + MaxChargeDuration);
    }

    WigglePhase = 0f;
  }

  protected override void OnDeactivate() {
    SetWiggleAngle(0f);
    ResetSlowOnDamage();
  }

  protected override bool ShouldAvoidInterruption()
    => State != StateType.Preparing;

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

  private void WhilePreparing() {
    Direction = Helper.DirectionToPlayer;
    float progress = Stopwatch.Progress(PrepareDuration);
    Helper.MoveWithVelocity(-1f * (1f - progress) * PrepareSpeed * Direction);
    Wiggle(PrepareWiggleSpeed * progress);
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

  private bool IsBullet(int layer)
    => ((1 << layer) & CollisionMask.BulletMask) != 0;

  private void DisableSlowOnDamage() {
    PreviousSlowOnDamage = HitPointEvents.SlowOnDamage;
    HitPointEvents.SlowOnDamage = false;
  }

  private void ResetSlowOnDamage() {
    HitPointEvents.SlowOnDamage = PreviousSlowOnDamage;
  }
}
