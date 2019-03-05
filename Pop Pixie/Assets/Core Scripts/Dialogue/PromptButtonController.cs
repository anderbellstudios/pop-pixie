using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PromptButtonController : MonoBehaviour {

  public GameObject PromptButtonPanel;
  public GameObject DefaultButton;
  public Text PveLabel, NveLabel;

  private IPromptButtonEventHandler EventHandler;

  public void Write (string pve, string nve, IPromptButtonEventHandler event_handler) {
    PveLabel.text = pve;
    NveLabel.text = nve;
    EventHandler = event_handler;
  }

  public void Show () {
    SetEnabled(true);
    EventSystem.current.SetSelectedGameObject( DefaultButton );
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    PromptButtonPanel.SetActive(state);
  }

  public void PveButtonPressed () {
    SendButtonEvent("positive");
  }

  public void NveButtonPressed () {
    SendButtonEvent("negative");
  }

  void SendButtonEvent (string button) {
    EventHandler.ButtonPressed(button);
  }
}
