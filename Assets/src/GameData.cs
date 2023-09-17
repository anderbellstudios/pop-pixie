using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : AData {
  public static GameData Current = new GameData();

  public override Dictionary<string, object> LocalDefaultDictionary() {
    return new Dictionary<string, object> {
      { "VERSION", "1.1" }
    };
  }

}
