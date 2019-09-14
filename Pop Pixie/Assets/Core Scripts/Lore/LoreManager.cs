using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour {

  public LoreWindowController LoreWindow;
  private ILoreEventHandler EventHandler;

  public void Open (string item_name, ILoreEventHandler event_handler=null) {
    string json = Resources.Load<TextAsset>(item_name).text;
    var item = LoreItem.ParseJSON(json);

    LoreItemData.RecordRead(item);

    MusicController.Current.SetVolume(0.25f);
    StateManager.SetState( State.Lore );
    EventHandler = event_handler;

    LoreWindow.Write( item.Text );
    LoreWindow.Show();
  }

	// Use this for initialization
	void Start () {
    LoreWindow.Hide();
	}
	
	// Update is called once per frame
	void Update () {
    if ( StateManager.Isnt( State.Lore ) )
      return;

    if ( WrappedInput.GetButtonDown("Cancel") ) {
      LoreWindow.Hide();

      if ( EventHandler != null )
        EventHandler.Closed();

      StateManager.SetState( State.Playing );
      MusicController.Current.SetVolume(1.0f);
    }
	}

}
