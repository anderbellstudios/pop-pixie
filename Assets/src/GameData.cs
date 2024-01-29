using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : AData {
  public static GameData Current = new GameData();

  private static bool OnDisk = false;

  public override Dictionary<string, object> LocalDefaultDictionary() {
    return new Dictionary<string, object> {
      { "VERSION", "1.1" }
    };
  }

  public static void LoadOrReset() {
    if (OnDisk) {
      Load();
    } else {
      Reset();
    }
  }

  public static void Save() {
    OnDisk = true;
    GameDataOperation.Write();
  }

  public static void Load() {
    OnDisk = true;
    GameDataOperation.Read();
  }

  public static void Reset() {
    OnDisk = false;
    Current.Clear();
  }

  public static bool Exists() => GameDataOperation.Exists();
  private static DataOperation GameDataOperation => new DataOperation(Current, "game");
}
