using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneData {

  public static void Save() {
    GameData.Current.Set(
      "scene",
      new SceneSerializer().Serialize()
    );
  }

  public static void Load() {
    GDCall.ExpectLoad();

    new SceneDeserializer(
      GameData.Current.Fetch("scene")
    ).Deserialize();
  }

}
