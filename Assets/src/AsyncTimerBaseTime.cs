using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncTimerBaseTime : AsyncTimer {
  public override float CurrentTime => Time.time;

  public override void SetAsSingleton() {
    AsyncTimer.BaseTime = this;
  }
}
