using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGremlinAttackAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  public float Damage;
  public float WiggleAmplitude;
  public float BeforeAttackDuration;
  public float GiveUpDuration = Mathf.Infinity;

  private bool Charging = false;
  private Transform WiggleTransform;

  void Start() {
    Helper.OnPlayerCollision(() => {
      bool isCounterAttack = Helper.DamagePlayer(Damage, true);

      if (isCounterAttack) {
        Helper.KillSelf();
      }

      OnFinish();
    });

    WiggleTransform = Helper.Transform.Find("Sprite");
  }

  protected override void OnActivate() {
    Charging = false;

    Helper.SetTimeout(
      () => {
        Charging = true;
      },
      BeforeAttackDuration
    );

    if (GiveUpDuration != Mathf.Infinity) {
      Helper.SetTimeout(OnFinish, GiveUpDuration);
    }
  }

  protected override void OnDeactivate() {
    SetWiggleAngle(0f);
  }

  protected override void WhileActive() {
    SetWiggleAngle(Random.Range(-WiggleAmplitude, WiggleAmplitude));

    if (Charging) {
      Helper.MoveTowardsPlayer(Speed);
    }
  }

  private void SetWiggleAngle(float angle) {
    WiggleTransform.rotation = Quaternion.Euler(0, 0, angle);
  }
}
