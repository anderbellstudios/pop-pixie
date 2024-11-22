using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayCloseToPlayerAI : AMovementEnemyAI {
  public float Speed;

  public NavigateToPlayerAI NavigateToPlayerAI;
  public MoveTowardsPlayerAI MoveTowardsPlayerAI;

  private LowPriorityBehaviour LowPriorityBehaviour;
  private bool CanMoveToPlayer = false;

  void Start() {
    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  protected override void OnActivate() {
    NavigateToPlayerAI.Speed = Speed;
    MoveTowardsPlayerAI.Speed = Speed;
  }

  protected override void WhileActive() {
    LowPriorityBehaviour.EveryNFrames(10, () => {
      CanMoveToPlayer = Helper.CanMoveToPlayer();
    });
  }

  protected override AMovementEnemyAI UseMovementAI()
    => CanMoveToPlayer ? MoveTowardsPlayerAI : NavigateToPlayerAI;
}
