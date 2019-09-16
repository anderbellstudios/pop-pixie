using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour {

  public LoreWindowController LoreWindow;

  private ILoreEventHandler EventHandler;
  private bool IsOpen;

  public void Open (LoreItem item, ILoreEventHandler event_handler=null) {
    EventHandler = event_handler;

    IsOpen = true;
    LoreWindow.Write( item.Text );
    LoreWindow.Show();
  }

	// Use this for initialization
	void Start () {
    IsOpen = false;
    LoreWindow.Hide();
	}
	
	// Update is called once per frame
	void Update () {
    if ( !IsOpen )
      return;

    if ( WrappedInput.GetButtonDown("Cancel") ) {
      LoreWindow.Hide();

      if ( EventHandler != null )
        EventHandler.Closed();

      IsOpen = false;
    }
	}

}
