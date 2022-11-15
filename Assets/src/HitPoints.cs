using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HitPoints : MonoBehaviour, ISerializableComponent {
  public string[] SerializableFields {
    get {
      List<string> fields = new List<string>();

      if (ShouldSave) {
        fields.Add("Current");
      }

      return fields.ToArray();
    }
  }

  public static HitPoints PlayerHitPoints;

  public bool IsPlayer = false;
  public bool ShouldSave = true;
  public float Maximum; 
  public bool InfiniteHP = false;
  public float Current; 
  public float RegenerateRate;
  public List<ACanBeDamagedArbiter> CanBeDamagedArbiters;
  public float LastDamaged;
  public bool Dead = false;
  public UnityEvent<HitPoints> OnUpdate, OnDecrease, OnBecomeZero;

  public void Cap() {
    // Make sure HP is between 0 and max
    Current = Mathf.Clamp( Current, 0, Maximum );
  }

  public float Set(float val) {
    Current = val;
    Cap();
    OnUpdate.Invoke(this);
    return Current;
  }

  public float Increase(float val) {
    Current += val;
    Cap();
    OnUpdate.Invoke(this);
    return Current;
  }

  public float Decrease(float val) {
    if (Dead)
      return 0.0f; // <-- Bypass callbacks

    if (!InfiniteHP) {
      Increase(-val * (float) (IsPlayer ? 1M - AssistModeData.DamageReduction : 1M));
    }

    OnDecrease.Invoke(this);

    if (Current == 0) {
      Dead = true;
      OnBecomeZero.Invoke(this);
    }

    return Current;
  }

  bool CanBeDamaged() {
    return CanBeDamagedArbiters.ToArray().All(
      arbiter => arbiter.CanBeDamaged(this)
    );
  }

  public float Damage(float val) {
    if (CanBeDamaged()) {
      LastDamaged = PlayingTime.time;
      return Decrease(val);
    }

    return -1.0f;
  }

  void Awake() {
    if (IsPlayer) {
      PlayerHitPoints = this;
    }
  }

	void Start() {
    GDCall.UnlessLoad(InitHitPoints);

    if (!ShouldSave)
      InitHitPoints();

    OnUpdate.Invoke(this);

    if (Current == 0) {
      Dead = true;
      OnBecomeZero.Invoke(this);
    }
	}

  public void InitHitPoints() {
    Current = Maximum;
    LastDamaged = -1000000f;
  }
	
	void Update() {
    if (!StateManager.Playing)
      return;

    Increase(RegenerateRate * Time.deltaTime);
	}
}
