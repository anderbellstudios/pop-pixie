using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour {

  public LoreWindowController LoreWindow;

  public void Open (string item_name) {
    string json = Resources.Load<TextAsset>(item_name).text;
    var item = LoreItem.ParseJSON(json);

    MusicController.Current.SetVolume(0.25f);
    StateManager.SetState( State.Lore );
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

    if ( Input.GetButton("Cancel") ) {
      LoreWindow.Hide();
      StateManager.SetState( State.Playing );
      MusicController.Current.SetVolume(1.0f);
    }
	}
}
