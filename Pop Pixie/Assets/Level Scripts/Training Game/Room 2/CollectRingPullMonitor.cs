using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectRingPullMonitor : AMonitor {
  public GameObject RingPull;
  public float DelayAfterPickUp = 2f;

  private bool RingPullCollected = false;

  public override void LocalUpdate() {
    if (Waiting && !RingPullCollected && RingPull == null) {
      RingPullCollected = true;
      Invoke("ConditionMet", DelayAfterPickUp);
    }
  }
}
