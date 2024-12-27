using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayCloseToPlayerAI : AMovementEnemyAI {
  [field: SerializeField]
  public override float Speed { get; set; }

  public AMovementEnemyAI CannotMoveToPlayerAI, CanMoveToPlayerAI;

  private LowPriorityBehaviour LowPriorityBehaviour;
  private bool CanMoveToPlayer = false;

  void Start() {
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  protected override void OnActivate() {
    CannotMoveToPlayerAI.Speed = Speed;
    CanMoveToPlayerAI.Speed = Speed;
  }

  protected override void WhileActive() {
    LowPriorityBehaviour.EveryNFrames(10, () => {
      if (!AvoidingInterruption) {
        CanMoveToPlayer = Helper.CanMoveToPlayer();
      }
    });
  }

  protected override AMovementEnemyAI UseMovementAI()
    => CanMoveToPlayer ? CanMoveToPlayerAI : CannotMoveToPlayerAI;
}
