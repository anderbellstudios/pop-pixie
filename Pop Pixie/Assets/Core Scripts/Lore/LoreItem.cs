using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoreItem {
  public string Text;
  public string UniqueId = null;

  public static LoreItem ParseJSON (string json) {
    return JsonUtility.FromJson<LoreItem>(json);
  }
}
