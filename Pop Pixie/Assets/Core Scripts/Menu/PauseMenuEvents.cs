using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuEvents : MonoBehaviour {

  public List<Button> InitialButtons;
  public List<GameObject> RemoveFromMinimalMenu;
  public PercentageButton MusicVolumeButton, SoundsVolumeButton;
  public TMP_Text ButtonIconsText;

  private List<Button> Buttons;

  EnumeratorButton<String> ButtonIconsButton;
  bool InFocus;

  void Start() {
    InFocus = true;

    MusicVolumeButton.Value = OptionsData.MusicVolume;
    MusicVolumeButton.UpdateValue();

    SoundsVolumeButton.Value = OptionsData.SoundsVolume;
    SoundsVolumeButton.UpdateValue();

    ButtonIconsButton = new EnumeratorButton<String>(
      new List<String>() { "Xbox", "PS" },
      ControllerTypeData.GetControllerType(),
      (type) => {
        ButtonIconsText.text = "Controller: " + type;
        ControllerTypeData.SetControllerType(type);
      }
    );

    if (PauseMenuData.MinimalPauseMenu) {
      RemoveFromMinimalMenu.ForEach(go => Destroy(go));
    }

    Buttons = InitialButtons.Where(b => b != null).ToList();
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
    ButtonIconsButton.Shift();
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
