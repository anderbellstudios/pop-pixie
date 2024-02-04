using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GameData : AData {
  public static GameData Current = new GameData();

  private static bool OnDisk = false;

  private const int LATEST_VERSION = 2;
  private const int HASH_VERSION = 2;

  public override Dictionary<string, object> LocalDefaultDictionary() {
    return new Dictionary<string, object> {
      { "VERSION", LATEST_VERSION }
    };
  }

  public override void AfterRead() {
    int version = 1;

    try {
      version = (int)Fetch("VERSION", orSetEqualTo: 1);
    } catch { }

    if (version >= HASH_VERSION && StoredHash() != ComputedHash()) {
      NotAnalytics.Current.Hit("game-data-edited-externally");
    }

    Set("VERSION", LATEST_VERSION);
  }

  public override void BeforeWrite() {
    Dictionary["HASH"] = ComputedHash();
  }

  private int ComputedHash() {
    int hash = 0;

    foreach (KeyValuePair<string, object> entry in Dictionary) {
      if (entry.Key != "HASH" && entry.Key != "VERSION") {
        string jsonValue = entry.Key + JsonConvert.SerializeObject(entry.Value);
        hash ^= jsonValue.GetHashCode();
      }
    }

    return hash;
  }

  private int StoredHash() => (int)Fetch("HASH", orSetEqualTo: 0);

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
