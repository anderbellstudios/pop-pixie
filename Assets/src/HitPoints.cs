using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class HitPoints : MonoBehaviour {
  public static HitPoints PlayerHitPoints;

  public class DamageContext {
    public HitPoints HitPoints;
    public float Damage;
    public bool IsDestructive;
  }

  public bool IsPlayer = false;
  public float Maximum;
  public bool InfiniteHP = false;
  public float Current;
  public float RegenerateRate;
  public List<ACanBeDamagedArbiter> CanBeDamagedArbiters;
  public ACounterAttackArbiter CounterAttackArbiter;
  public float LastDamaged;
  public float LastDamageAmount;
  public bool Dead = false;
  public UnityEvent<HitPoints> OnUpdate,
    OnDecrease,
    OnBecomeZero,
    OnCounterAttack;

  public void Cap() {
    // Make sure HP is between 0 and max
    Current = Mathf.Clamp(Current, 0, Maximum);
  }

  public void Set(float hp) {
    Current = hp;
    Cap();
    OnUpdate.Invoke(this);
  }

  public void Increase(float amount) {
    Current += amount;
    Cap();
    OnUpdate.Invoke(this);
  }

  public void Decrease(float damage, bool ignoreDamageReduction = false) {
    if (Dead)
      return;

    if (!InfiniteHP) {
      float damageReductionFactor = ignoreDamageReduction
        ? 1f
        : (float)(IsPlayer ? 1M - AssistModeData.DamageReduction : 1M);
      Increase(-damage * damageReductionFactor);
    }

    OnDecrease.Invoke(this);

    if (Current == 0) {
      Dead = true;
      OnBecomeZero.Invoke(this);
    }
  }

  bool CanBeDamaged(float damage, bool isDestructive) {
    DamageContext ctx = new DamageContext {
      HitPoints = this,
      Damage = damage,
      IsDestructive = isDestructive,
    };

    return CanBeDamagedArbiters.All(arbiter => arbiter.CanBeDamaged(ctx));
  }

  bool IsCounterAttack() {
    return CounterAttackArbiter != null && CounterAttackArbiter.IsCounterAttack();
  }

  // Returns true on counter attack
  public bool Damage(
    float damage,
    bool canBeCounterAttacked = false,
    bool isDestructive = false,
    bool ignoreCanBeDamaged = false,
    bool ignoreDamageReduction = false
  ) {
    if (canBeCounterAttacked && IsCounterAttack()) {
      OnCounterAttack.Invoke(this);
      return true;
    }

    if (ignoreDamageReduction || CanBeDamaged(damage, isDestructive)) {
      LastDamaged = PlayingTime.time;
      LastDamageAmount = damage;
      Decrease(damage, ignoreDamageReduction: ignoreDamageReduction);
    }

    return false;
  }

  public void Kill() {
    Damage(Mathf.Infinity, ignoreCanBeDamaged: true, ignoreDamageReduction: true);
  }

  public static float InitStartOrder = 1;
  public static float UpdateStartOrder => OrderedStart.After(InitStartOrder);

  void Awake() {
    if (IsPlayer) {
      PlayerHitPoints = this;
    }

    OrderedStart.Add(
      () => {
        Current = Maximum;
        LastDamaged = -Mathf.Infinity;
        LastDamageAmount = 0f;
      },
      InitStartOrder
    );

    OrderedStart.Add(
      () => {
        OnUpdate.Invoke(this);
      },
      UpdateStartOrder
    );
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    Increase(RegenerateRate * Time.deltaTime);
  }
}
