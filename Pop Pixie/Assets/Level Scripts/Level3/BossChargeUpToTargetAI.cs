using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeUpToTargetAI : AEnemyAI {
  
  public float Speed;
  public float Distance;
  public AEnemyAI WhenNextToTarget;

  public override void WhileInControl() {
    if ( TargetDistance() > Distance ) {
      ApplyMovement( Speed * TargetDirection() );
    } else {
      RelinquishControlTo( WhenNextToTarget );
    }
  }

}
