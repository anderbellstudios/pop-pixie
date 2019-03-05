using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour {

  public LoreWindowController LoreWindow;

  public void Open (string item_name) {
    string json = Resources.Load<TextAsset>(item_name).text;
    var item = LoreItem.ParseJSON(json);

    LoreWindow.Write( item.Text );
    LoreWindow.Show();
  }

	// Use this for initialization
	void Start () {
    LoreWindow.Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
