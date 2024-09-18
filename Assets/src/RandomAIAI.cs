using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIAI : AEnemyAI {
  public List<AEnemyAI> AIs;
  public float Interval = 0f;
  public int LastAI = -1;

  public override void ControlGained() {
    if (Interval > 0) {
      SetInterval(RelinquishControlToRandomAI, Interval);
    } else {
      RelinquishControlToRandomAI();
    }
  }

  private void RelinquishControlToRandomAI() {
    int i = LastAI;

    while (i == LastAI) {
      i = Random.Range(0, AIs.Count);
    }

    LastAI = i;

    RelinquishControlTo(AIs[i]);
  }
}
