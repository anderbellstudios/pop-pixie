using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuEvents : MonoBehaviour {

  public List<Button> Buttons;
  public PercentageButton MusicVolumeButton, SoundsVolumeButton;

  bool InFocus;

  void Start() {
    InFocus = true;

    MusicVolumeButton.Value = OptionsData.MusicVolume;
    MusicVolumeButton.UpdateValue();

    SoundsVolumeButton.Value = OptionsData.SoundsVolume;
    SoundsVolumeButton.UpdateValue();
  }

  void Update() {
    if ( InFocus && WrappedInput.GetButtonDown("Pause") ) {
      Resume();
    }
  }

  public void Focus() {
    SetButtonsEnabled(true);
    Buttons[0].Select();
    Buttons[0].OnSelect(null);
    InFocus = true;
  }

  public void Resume() {
    UnloadScene();
    StateManager.SetState( State.Playing );
  }

  public void DiscoveredItems() {
    SetButtonsEnabled(false);
    DiscoveredItemsMenuEvents.ParentMenu = this;
    SceneManager.LoadScene( "Discovered Items Menu", LoadSceneMode.Additive );
    InFocus = false;
  }

  public void MusicVolumeChanged( decimal volume ) {
    OptionsData.MusicVolume = volume;
  }

  public void SoundsVolumeChanged( decimal volume ) {
    OptionsData.SoundsVolume = volume;
  }

  public void CalibrateController() {
    UnloadScene();
    SceneManager.LoadScene( "Controller Calibration", LoadSceneMode.Additive );
  }

  public void QuitGame() {
    WrappedApplication.Quit();
  }

  void SetButtonsEnabled( bool enabled ) {
    Buttons.ForEach( button => button.interactable = enabled );
  }

  void UnloadScene() {
    SceneManager.UnloadSceneAsync("Pause Menu");
  }

}
