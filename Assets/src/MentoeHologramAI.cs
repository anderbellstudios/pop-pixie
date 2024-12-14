using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramAI : AMovementEnemyAI {
  public AMovementEnemyAI AttackingAI;

  void Start() {
    Activate();
  }

  protected override AMovementEnemyAI UseMovementAI()
    => AttackingAI;
}
