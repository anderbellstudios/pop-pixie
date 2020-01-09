using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : AData {
  public static GameData Current = new GameData();

  public override Dictionary<string, dynamic> LocalDefaultDictionary() {
    return new Dictionary<string, dynamic> {
      { "VERSION", "1.1" }
    };
  }

}
