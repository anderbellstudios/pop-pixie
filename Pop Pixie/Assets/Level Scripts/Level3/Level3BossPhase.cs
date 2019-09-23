using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3BossPhase : APhase {

  public Level3JumpDownAnimation JumpAnimation;
  public Rigidbody2D rb;
  public AEnemyAI InitialAI;

	public override void LocalBegin () {
    JumpAnimation.Perform( JumpFinished );
  }

  void JumpFinished() {
    Invoke( "StartAI", 0.5f );
  }

  void StartAI() {
    rb.isKinematic = false;
    InitialAI.GainControl();
  }

}
