using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData : AData {
  public static GameData Current = new GameData();

  public override Dictionary<string, dynamic> LocalDefaultDictionary() {
    return new Dictionary<string, dynamic> {
      { "VERSION", "1.1" }
    };
  }

  public void Save() {
    Dictionary["scene"] = new SceneSerializer().Serialize();
  }

  public void Load() {
    GDCall.ExpectLoad();
    new SceneDeserializer( Dictionary["scene"] ).Deserialize();
  }

}
