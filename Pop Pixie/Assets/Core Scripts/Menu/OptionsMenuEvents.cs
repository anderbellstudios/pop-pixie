using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;

public class OptionsMenuEvents : AMenu {
  public StepperInput MusicVolumeStepper, SoundsVolumeStepper, ControllerIconsStepper;
  public GameObject ControllerIconsGameObject;
  public AMenu GraphicsSettingsMenu;

  private EnumeratorButton<String> ButtonIconsButton;

  public override void LocalStart() {
    MusicVolumeStepper.Options = SoundsVolumeStepper.Options =
      Enumerable.Range(0, 11).Select(n => String.Format("{0}%", n * 10)).ToList();

    MusicVolumeStepper.Value = (int) (OptionsData.MusicVolume * 10);
    MusicVolumeStepper.UpdateLabel();

    SoundsVolumeStepper.Value = (int) (OptionsData.SoundsVolume * 10);
    SoundsVolumeStepper.UpdateLabel();

    MusicVolumeStepper.OnChange.AddListener(MusicVolumeChanged);
    SoundsVolumeStepper.OnChange.AddListener(SoundsVolumeChanged);

    ControllerIconsStepper.Value = ControllerIconsStepper.ValueForLabel(ControllerTypeData.GetControllerType());
    ControllerIconsStepper.UpdateLabel();

    ControllerIconsStepper.OnChange.AddListener(ControllerIconsChanged);
  }

  public override void LocalUpdate() {
    if (WrappedInput.Player.controllers.Joysticks.Count() >= 1)
      ControllerIconsGameObject.SetActive(true);
  }

  public void GraphicsSettings() {
    OpenNestedMenu(GraphicsSettingsMenu);
  }

  public void MusicVolumeChanged(int index, string label) {
    OptionsData.MusicVolume = ((decimal) index) * 0.1M;
  }

  public void SoundsVolumeChanged(int index, string label) {
    OptionsData.SoundsVolume = ((decimal) index) * 0.1M;
  }

  public void ControllerIconsChanged(int index, string label) {
    ControllerTypeData.SetControllerType(label);
  }
}
