using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuEvents : AMenu {
  public PercentageButton MusicVolumeButton, SoundsVolumeButton;
  public TMP_Text ButtonIconsText;
  public AMenu GraphicsSettingsMenu;

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

  public void GraphicsSettings() {
    OpenNestedMenu(GraphicsSettingsMenu);
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
