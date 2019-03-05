using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreWindowController : MonoBehaviour {

  public Text TextBox;
  public GameObject LoreWindow;

  public void Write (string text) {
    TextBox.text = text;
  }

  public void Show () {
    SetEnabled(true);
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    LoreWindow.SetActive(state);
  }
	// Use this for initialization
	void Start () {
    Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
