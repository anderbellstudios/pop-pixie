using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2OggPlayer : AInspectable {
  public LoreItem LoreItem;
  public SongHopper SongHopper;

  public override void OnInspect() {
    // Dialogue.Play("Dialogue/OggPlayer", this);
    Debug.Log("Not implemented");

    // StateManager.SetState( State.Lore );

    // LoreManager.Current.Open(LoreItem, () => {
    //   SongHopper.Stop();
    // });

    // SongHopper.Hop();
  }
}
