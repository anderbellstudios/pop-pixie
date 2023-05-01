using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectRingPullMonitor : AMonitor {
  public float DelayAfterPickUp = 2f;

  private int PreviousRingPullCount;

  public override void LocalAwake() {
    PreviousRingPullCount = RingPullsData.Amount();
  }

  public override void LocalUpdate() {
    if (Waiting && RingPullsData.Amount() > PreviousRingPullCount) {
      PreviousRingPullCount = RingPullsData.Amount();
      AsyncTimer.PlayingTime.SetTimeout(ConditionMet, DelayAfterPickUp);
    }
  }
}
