using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData {
  public static GameData Current = new GameData();

  public Dictionary<string, dynamic> Dictionary = new Dictionary<string, dynamic> {
    { "VERSION", "1.1" }
  };

  // Remove-me
  public int LevelId;

  public static void Save() {
    Current.Dictionary["scene"] = new SceneSerializer().Serialize();
  }

}
