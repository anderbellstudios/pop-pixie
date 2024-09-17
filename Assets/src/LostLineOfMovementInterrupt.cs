using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostLineOfMovementInterrupt : AInterrupt {
  public float Delay;
  public AEnemyAI WhenLostLineOfMovement;

  private Stopwatch Stopwatch;

  public override Type OnlyAIsMatching() {
    return typeof(IRequiresLineOfMovementAI);
  }

  public override void LocalStart() {
    Stopwatch = new Stopwatch.PlayingTime();
  }

  public override bool ShouldInterrupt(AEnemyAI ai) {
    if (ai.LineOfMovement())
      Stopwatch.Reset();

    return Stopwatch.Time() >= Delay;
  }

  public override AEnemyAI InterruptAI() {
    return WhenLostLineOfMovement;
  }
}
