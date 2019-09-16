using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuEvents : MonoBehaviour {

  public List<Button> Buttons;

  void Update() {
    if ( WrappedInput.GetButtonDown("Pause") ) {
      Resume();
    }
  }

  public void Focus() {
    Buttons.ForEach( button => button.interactable = true );
    Buttons[0].Select();
    Buttons[0].OnSelect(null);
  }

  public void Resume() {
    SceneManager.UnloadSceneAsync("Pause Menu");
    StateManager.SetState( State.Playing );
  }

  public void DiscoveredItems() {
    Buttons.ForEach( button => button.interactable = false );
    DiscoveredItemsMenuEvents.ParentMenu = this;
    SceneManager.LoadScene( "Discovered Items Menu", LoadSceneMode.Additive );
  }

  public void QuitGame() {
    WrappedApplication.Quit();
  }

}
