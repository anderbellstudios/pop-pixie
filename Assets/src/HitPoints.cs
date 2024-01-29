﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HitPoints : MonoBehaviour {
  public static HitPoints PlayerHitPoints;

  public bool IsPlayer = false;
  public float Maximum;
  public bool InfiniteHP = false;
  public float Current;
  public float RegenerateRate;
  public List<ACanBeDamagedArbiter> CanBeDamagedArbiters;
  public ACounterAttackArbiter CounterAttackArbiter;
  public float LastDamaged;
  public bool Dead = false;
  public UnityEvent<HitPoints> OnUpdate, OnDecrease, OnBecomeZero, OnCounterAttack;

  public void Cap() {
    // Make sure HP is between 0 and max
    Current = Mathf.Clamp(Current, 0, Maximum);
  }

  public void Set(float val) {
    Current = val;
    Cap();
    OnUpdate.Invoke(this);
  }

  public void Increase(float val) {
    Current += val;
    Cap();
    OnUpdate.Invoke(this);
  }

  public void Decrease(float val) {
    if (Dead)
      return;

    if (!InfiniteHP) {
      Increase(-val * (float)(IsPlayer ? 1M - AssistModeData.DamageReduction : 1M));
    }

    OnDecrease.Invoke(this);

    if (Current == 0) {
      Dead = true;
      OnBecomeZero.Invoke(this);
    }
  }

  bool CanBeDamaged() {
    return CanBeDamagedArbiters.ToArray().All(
      arbiter => arbiter.CanBeDamaged(this)
    );
  }

  bool IsCounterAttack() {
    return CounterAttackArbiter != null && CounterAttackArbiter.IsCounterAttack();
  }

  // Returns true on counter attack
  public bool Damage(float val, bool canBeCounterAttacked = false) {
    if (canBeCounterAttacked && IsCounterAttack()) {
      OnCounterAttack.Invoke(this);
      return true;
    }

    if (CanBeDamaged()) {
      LastDamaged = PlayingTime.time;
      Decrease(val);
    }

    return false;
  }

  void Awake() {
    if (IsPlayer) {
      PlayerHitPoints = this;
    }
  }

  void Start() {
    Current = Maximum;
    LastDamaged = -1000000f;
    OnUpdate.Invoke(this);
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    Increase(RegenerateRate * Time.deltaTime);
  }
}
