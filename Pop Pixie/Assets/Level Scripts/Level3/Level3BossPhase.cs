using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3BossPhase : APhase {

  public Level3JumpDownAnimation JumpAnimation;

	public override void LocalBegin () {
    JumpAnimation.Perform( JumpFinished );
  }

  void JumpFinished() {
    Debug.Log("I done jumping");
  }

}
