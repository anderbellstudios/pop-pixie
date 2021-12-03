using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectPiecesOfIntelMonitor : AMonitor {
  public List<PieceOfIntelSprite> PiecesOfIntel;

  public override bool TestCondition() {
    if (StateManager.Isnt( State.Playing ))
      return false;

    return PiecesOfIntel.All(x => x.Collected);
  }
}
