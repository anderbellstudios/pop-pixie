using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectPieceOfIntelMonitor : AMonitor {
  public PieceOfIntelSprite PieceOfIntel;

  public override bool TestCondition() {
    if (!StateManager.Playing)
      return false;

    return PieceOfIntel.Collected;
  }
}
