using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetaMonitor : AMonitor {
  public enum QuantifierEnum { All, Any };

  public List<AMonitor> Monitors;
  public QuantifierEnum Quantifier;

  public override void BeganWaiting() {
    Monitors.ForEach(m => m.StartWaiting());
  }

  public override bool TestCondition() {
    return Quantifier == QuantifierEnum.All
      ? Monitors.All(m => m.TestCondition())
      : Monitors.Any(m => m.TestCondition());
  }
}
