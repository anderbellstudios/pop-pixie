using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoreItemData {

  public static void RecordRead(LoreItem loreItem) {
    if (loreItem.UniqueId == null)
      return;

    if (AlreadyRead(loreItem))
      return;

    List<String> readLoreItems = ReadLoreItems();
    readLoreItems.Add(loreItem.UniqueId);
    GameData.Current.Set("lore-item-ids", readLoreItems);
  }

  public static List<String> ReadLoreItems() {
    dynamic loreItemIds = GameData.Current.Fetch("lore-item-ids", orSetEqualTo: new List<String>());
    return CoerceJson.To<List<String>>(loreItemIds);
  }

  static bool AlreadyRead(LoreItem loreItem) {
    return AlreadyRead(loreItem.UniqueId);
  }

  static bool AlreadyRead(string loreItemId) {
    return ReadLoreItems().Any(id => id == loreItemId);
  }

}
