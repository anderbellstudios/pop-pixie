using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsDestroyedMonitor : AMonitor {
  public Transform OptionalContainer;

  public override bool TestCondition() {
    return EnemyUtils.InContainer(OptionalContainer).All(t => EnemyUtils.IsDead(t));
  }
}
