using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButtonExplanation : MonoBehaviour {
  public AMenu Menu;
  public TMP_Text Text;

  void Start() {
    SelectionChangeListener.AddListener((currentSelected, previousSelected) => {
      Text.text = GetExplanation(currentSelected);
    }, gameObject);
  }

  private string GetExplanation(GameObject go) {
    if (go == null)
      return "";
    MenuButton mb = go.GetComponent<MenuButton>();
    if (mb == null || mb.Menu != Menu)
      return "";
    return mb.Explanation;
  }
}
