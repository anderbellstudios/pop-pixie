using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

public class OptionsMenuEvents : AMenu {
  public StepperInput ControllerIconsStepper;
  public GameObject ControllerIconsGameObject;
  public AMenu GraphicsSettingsMenu,
    AudioSettingsMenu;

  private EnumeratorButton<String> ButtonIconsButton;

  protected override void LocalStart() {
    ControllerIconsStepper.Value = ControllerIconsStepper.ValueForLabel(
      ControllerTypeData.GetControllerType()
    );
    ControllerIconsStepper.UpdateLabel();

    ControllerIconsStepper.OnChange.AddListener(ControllerIconsChanged);
  }

  protected override void LocalUpdate() {
    if (WrappedInput.Player.controllers.Joysticks.Count >= 1)
      ControllerIconsGameObject.SetActive(true);
  }

  public void GraphicsSettings() {
    OpenNestedMenu(GraphicsSettingsMenu);
  }

  public void AudioSettings() {
    OpenNestedMenu(AudioSettingsMenu);
  }

  private void ControllerIconsChanged(int index, string label) {
    ControllerTypeData.SetControllerType(label);
  }
}
