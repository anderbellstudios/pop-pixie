using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleRandomAI : AMovementEnemyAI {
  public float DelayBetweenAIs;
  public bool InitialDelay;
  public List<AMovementEnemyAI> AIs;

  private int CurrentIndex = -1;
  private int LastIndex = -1;

  protected override AMovementEnemyAI UseMovementAI()
    => CurrentIndex == -1 ? null : AIs[CurrentIndex];

  protected override void OnActivate() {
    ScheduleChangeAI();
  }

  protected override void OnChildFinish(AEnemyAI2 child) {
    ScheduleChangeAI();
  }

  private void ScheduleChangeAI() {
    bool instant = DelayBetweenAIs == 0f || (
      IsFirst && !InitialDelay
    );

    LastIndex = CurrentIndex;

    if (instant) {
      ChangeAI();
    } else {
      ClearAI();
      Helper.SetTimeout(ChangeAI, DelayBetweenAIs);
    }
  }

  private void ChangeAI() {
    int aiCount = AIs.Count;

    if (LastIndex == -1 || aiCount < 2) {
      CurrentIndex = Random.Range(0, aiCount);
      return;
    }

    // Random index excluding LastIndex
    int nextIndex = Random.Range(0, aiCount - 1);
    if (nextIndex >= LastIndex)
      nextIndex++;

    CurrentIndex = nextIndex;
  }

  private void ClearAI() {
    CurrentIndex = -1;
  }

  private bool IsFirst => LastIndex == -1;
}
