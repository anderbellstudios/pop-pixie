using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LoreWindowController : MonoBehaviour {

  public TMP_Text TextBox;
  public RectTransform TextBoxTransform;
  public GameObject LoreWindow;
  public GameObject ScrollBar;

  public void Write (string text) {
    TextBox.text = text;
    TextBoxTransform.sizeDelta = new Vector2(
      0,
      TextBox.GetPreferredValues().y
    );
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
