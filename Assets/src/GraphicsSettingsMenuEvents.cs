using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GraphicsSettingsMenuEvents : AMenu {
  public StepperInput ResolutionStepper, FullscreenStepper;

  private Resolution Resolution;
  private bool Fullscreen;

  public override void LocalStart() {
    ResolutionStepper.Options = Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();

    ResolutionStepper.Value = Array.FindIndex(Screen.resolutions, resolution =>
      (resolution.width == ResolutionData.Width) && (resolution.height == ResolutionData.Height)
    );

    if (ResolutionStepper.Value == -1)
      ResolutionStepper.Value = Screen.resolutions.Count() - 1;

    Resolution = Screen.resolutions[ResolutionStepper.Value];

    ResolutionStepper.OnChange.AddListener(ResolutionChanged);
    ResolutionStepper.UpdateLabel();

    FullscreenStepper.Value = ResolutionData.Fullscreen ? 1 : 0;
    FullscreenStepper.OnChange.AddListener(FullscreenChanged);
    FullscreenStepper.UpdateLabel();
    Fullscreen = ResolutionData.Fullscreen;
  }

  public void ResolutionChanged(int index, string label) {
    Resolution = Screen.resolutions[index];
  }

  public void FullscreenChanged(int index, string label) {
    Fullscreen = index == 1;
  }

  public void SaveAndApply() {
    ResolutionData.Width = Resolution.width;
    ResolutionData.Height = Resolution.height;
    ResolutionData.Fullscreen = Fullscreen;
    ResolutionData.Apply();
  }
}
