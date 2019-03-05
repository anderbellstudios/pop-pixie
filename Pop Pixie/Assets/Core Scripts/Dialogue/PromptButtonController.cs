using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptButtonController : MonoBehaviour {

  public GameObject PromptButtonPanel;

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
