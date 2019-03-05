using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreWindowController : MonoBehaviour {

  public Text TextBox;
  public GameObject LoreWindow;
  public Scrollbar Scroll;

  public void Write (string text) {
    TextBox.text = text;
  }

  public void Show () {
    SetEnabled(true);
    Scroll.value = 1f;
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    LoreWindow.SetActive(state);
  }
}
