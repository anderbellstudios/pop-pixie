using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuEvents : MonoBehaviour {

  public List<Button> Buttons;
  public PercentageButton MusicVolumeButton, SoundsVolumeButton;
  public TMP_Text ButtonIconsText;

  bool InFocus;

  void Start() {
    InFocus = true;

    MusicVolumeButton.Value = OptionsData.MusicVolume;
    MusicVolumeButton.UpdateValue();

    SoundsVolumeButton.Value = OptionsData.SoundsVolume;
    SoundsVolumeButton.UpdateValue();

    UpdateButtonIconsText();
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

  public void ToggleButtonIcons() {
    string oldType = ControllerTypeData.GetType();
    ControllerTypeData.SetType( oldType == "Xbox" ? "PS" : "Xbox" );
    UpdateButtonIconsText();
  }

  void UpdateButtonIconsText() {
    ButtonIconsText.text = "Controller: " + ControllerTypeData.GetType();
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
