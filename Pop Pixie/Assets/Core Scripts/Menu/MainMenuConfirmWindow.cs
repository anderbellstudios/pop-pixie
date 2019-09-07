using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuConfirmWindow : MonoBehaviour {

  public Canvas WindowCanvas;
  public Button CancelButton, OverwriteButton;
  public Button NewGameButton, ContinueButton;

  public void Show() {
    SetVisible(true);
  }

  public void Hide() {
    SetVisible(false);
  }

  void SetVisible( bool visible ) {
    NewGameButton.interactable = !visible;
    ContinueButton.interactable = !visible;

    CancelButton.interactable = visible;
    OverwriteButton.interactable = visible;

    WindowCanvas.enabled = visible;

    Button button;

    if ( visible ) {
      button = CancelButton;
    } else {
      button = NewGameButton;
    }

    button.Select();
    button.OnSelect(null);
  }

}
