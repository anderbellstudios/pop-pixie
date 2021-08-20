using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RegisteredLoreItems")]
[System.Serializable]
public class RegisteredLoreItems : ScriptableObject {
  public List<LoreItem> LoreItems;

  public LoreItem Find(string id) {
    foreach (LoreItem loreItem in LoreItems) {
      if (loreItem.UniqueId == id)
        return loreItem;
    }

    return null;
  }
}
