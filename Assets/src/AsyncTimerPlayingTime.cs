using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GlobalPlayingTime = PlayingTime;

public class AsyncTimerPlayingTime : AsyncTimer {
  public override float CurrentTime => GlobalPlayingTime.time;

  public override void SetAsSingleton() {
    AsyncTimer.PlayingTime = this;
  }
}
