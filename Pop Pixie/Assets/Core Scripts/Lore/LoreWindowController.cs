using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LoreWindowController : MonoBehaviour {

  public TMP_Text Title, TextBox;
  public GameObject LoreWindow;
  public GameObject ScrollBar;

  public void SetTitle(string title) {
    Title.text = title;
  }

  public void SetText(string text) {
    TextBox.text = text;
  }

  public void Show () {
    SetEnabled(true);
    ScrollBar.GetComponent<Scrollbar>().value = 1f;
    EventSystem.current.SetSelectedGameObject( ScrollBar );
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    LoreWindow.SetActive(state);
  }
}
