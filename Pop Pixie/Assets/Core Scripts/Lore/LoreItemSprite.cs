using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreItemSprite : AInspectable {
  public LoreItem LoreItem;

  public override void OnInspect() {
    LoreItemData.RecordRead(LoreItem);
    StateManager.SetState( State.Lore );

    LoreManager.Current.Open(LoreItem, () => {
      StateManager.SetState( State.Playing );
    });
  }
}
