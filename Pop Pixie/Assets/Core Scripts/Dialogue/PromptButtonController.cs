using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptButtonController : MonoBehaviour {

  public GameObject PromptButtonPanel;
  public Text PveLabel, NveLabel;

  public void Write (string pve, string nve) {
    PveLabel.text = pve;
    NveLabel.text = nve;
  }

  public void Show () {
    SetEnabled(true);
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    PromptButtonPanel.SetActive(state);
  }
}
