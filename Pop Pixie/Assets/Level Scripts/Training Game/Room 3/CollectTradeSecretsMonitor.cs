using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectTradeSecretsMonitor : AMonitor {
  public List<TradeSecretSprite> TradeSecrets;

  public override bool TestCondition() {
    if (StateManager.Isnt( State.Playing ))
      return false;

    return TradeSecrets.All(x => x.Collected);
  }
}
