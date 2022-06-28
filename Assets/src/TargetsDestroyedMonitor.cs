using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsDestroyedMonitor : AMonitor {
  public List<GameObject> Targets;

  public override bool TestCondition() {
    return Targets.All(t => EnemyUtils.IsDead(t));
  }
}
