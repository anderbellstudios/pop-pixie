using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIAI : AEnemyAI {

  public List<AEnemyAI> AIs;

  public override void ControlGained() {
    RelinquishControlTo( RandomAI() );
  }

  AEnemyAI RandomAI() {
    var i = Random.Range(0, AIs.Count);
    return AIs[i];
  }

}
