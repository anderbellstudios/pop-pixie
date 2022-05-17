using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIAI : AEnemyAI {
  public List<AEnemyAI> AIs;
  public float Interval = 0f;
  public int LastAI = -1;

  private IntervalTimer IntervalTimer;

  public override void ControlGained() {
    if (Interval > 0) {
      IntervalTimer = new IntervalTimer() {
        TimeClass = "PlayingTime",
        Interval = Interval
      };

      IntervalTimer.Reset();
    } else {
      RelinquishControlToRandomAI();
    }
  }

  public override void WhileInControl() {
    if (Interval > 0) {
      IntervalTimer.IfElapsed(RelinquishControlToRandomAI);
    }
  }

  void RelinquishControlToRandomAI() {
    int i = LastAI;

    while (i == LastAI) {
      i = Random.Range(0, AIs.Count);
    }

    LastAI = i;

    RelinquishControlTo(AIs[i]);
  }
}
