using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuEvents : AMenu {
  public PercentageButton MusicVolumeButton, SoundsVolumeButton;
  public TMP_Text ButtonIconsText;
  public AMenu DiscoveredItemsMenu;

  private EnumeratorButton<String> ButtonIconsButton;

  public override void LocalStart() {
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
  }

  public override void LocalClose() {
    StateManager.SetState( State.Playing );
    SceneManager.UnloadSceneAsync("Pause Menu");
  }

  public void Resume() {
    Close();
  }

  public void DiscoveredItems() {
    OpenNestedMenu(DiscoveredItemsMenu);
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
}
