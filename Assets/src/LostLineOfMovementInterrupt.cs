using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostLineOfMovementInterrupt : AInterrupt {

  public float Delay;
  public AEnemyAI WhenLostLineOfMovement;

  IntervalTimer Timer;

  public override Type OnlyAIsMatching() {
    return typeof(IRequiresLineOfMovementAI);
  }

  public override void LocalStart() {
    Timer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = Delay
    };

    Timer.Start();
  }

  public override bool ShouldInterrupt(AEnemyAI ai) {
    if (ai.LineOfMovement())
      Timer.Reset();

    return Timer.Elapsed();
  }

  public override AEnemyAI InterruptAI() {
    return WhenLostLineOfMovement;
  }

}
