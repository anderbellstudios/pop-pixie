using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIAI : AEnemyAI {

  public List<AEnemyAI> AIs;
  public int LastAI = -1;

  public override void ControlGained() {
    RelinquishControlTo( RandomAI() );
  }

  AEnemyAI RandomAI() {
    int i = LastAI;

    while ( i == LastAI ) {
      i = Random.Range(0, AIs.Count);
    }

    LastAI = i;

    return AIs[i];
  }

}
