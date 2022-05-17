using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreItemSprite : AInspectable {
  public LoreItem LoreItem;

  public override void OnInspect() {
    LoreItemData.RecordRead(LoreItem);
    StateManager.AddState(State.NotPlaying);

    LoreManager.Current.Open(LoreItem, () => {
      StateManager.RemoveState(State.NotPlaying);
    });
  }
}
