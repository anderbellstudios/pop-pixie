using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoreItemData {

  public static void RecordRead( LoreItem loreItem ) {
    if ( loreItem.UniqueId == null )
      return;

    if ( AlreadyRead(loreItem) )
      return;

    ReadLoreItems().Add(loreItem);
  }

  public static List<LoreItem> ReadLoreItems() {
    return GameData.Fetch(
      "lore-items", 
      orSetEqualTo: new List<LoreItem>()
    );
  }

  static bool AlreadyRead( LoreItem loreItem ) {
    return ReadLoreItems().Any( l => l.UniqueId == loreItem.UniqueId );
  }

}
