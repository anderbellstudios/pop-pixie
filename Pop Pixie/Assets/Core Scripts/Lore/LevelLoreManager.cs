using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoreManager : MonoBehaviour, ILoreEventHandler {

  public static LevelLoreManager Current;

  public LoreManager LoreManager;
  private ILoreEventHandler EventHandler;

  void Start() {
    Current = this;
  }

  public void Open (string item_name, ILoreEventHandler event_handler=null) {
    string json = Resources.Load<TextAsset>(item_name).text;
    var item = LoreItem.ParseJSON(json);

    LoreItemData.RecordRead(item);

    MusicController.Current.SetVolume(0.25f);
    StateManager.SetState( State.Lore );
    EventHandler = event_handler;

    LoreManager.Open( item, this );
  }

	public void Closed() {
    if ( EventHandler != null )
      EventHandler.Closed();

    StateManager.SetState( State.Playing );
    MusicController.Current.SetVolume(1.0f);
	}

}
