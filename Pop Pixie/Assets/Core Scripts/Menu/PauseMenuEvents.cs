using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuEvents : MonoBehaviour {

  public List<Button> Buttons;
  bool InFocus;

  void Start() {
    InFocus = true;
  }

  void Update() {
    if ( InFocus && WrappedInput.GetButtonDown("Pause") ) {
      Resume();
    }
  }

  public void Focus() {
    Buttons.ForEach( button => button.interactable = true );
    Buttons[0].Select();
    Buttons[0].OnSelect(null);
    InFocus = true;
  }

  public void Resume() {
    SceneManager.UnloadSceneAsync("Pause Menu");
    StateManager.SetState( State.Playing );
  }

  public void DiscoveredItems() {
    Buttons.ForEach( button => button.interactable = false );
    DiscoveredItemsMenuEvents.ParentMenu = this;
    SceneManager.LoadScene( "Discovered Items Menu", LoadSceneMode.Additive );
    InFocus = false;
  }

  public void QuitGame() {
    WrappedApplication.Quit();
  }

}
