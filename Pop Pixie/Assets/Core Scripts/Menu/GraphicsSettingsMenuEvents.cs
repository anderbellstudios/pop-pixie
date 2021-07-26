using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GraphicsSettingsMenuEvents : AMenu {
  public TMP_Text ResolutionText, FullscreenText;

  private EnumeratorButton<Resolution> ResolutionButton;
  private EnumeratorButton<Boolean> FullscreenButton;

  private Resolution Resolution;
  private bool Fullscreen;

  public override void LocalStart() {
    ResolutionButton = new EnumeratorButton<Resolution>(
      values: Screen.resolutions.ToList(),

      initialValue: new Resolution() {
        width = ResolutionData.Width,
        height = ResolutionData.Height,
        refreshRate = 60
      },

      onChange: (resolution) => {
        ResolutionText.text = $"Resolution: {resolution.width}x{resolution.height}";
        Resolution = resolution;
      }
    );

    FullscreenButton = new EnumeratorButton<Boolean>(
      values: new List<Boolean>() { true, false },
      initialValue: ResolutionData.Fullscreen,

      onChange: (fullscreen) => {
        FullscreenText.text = "Mode: " + (fullscreen ? "Fullscreen" : "Windowed");
        Fullscreen = fullscreen;
      }
    );
  }

  public void ShiftResolution() {
    ResolutionButton.Shift();
  }

  public void ShiftFullscreen() {
    FullscreenButton.Shift();
  }

  public void SaveAndApply() {
    ResolutionData.Width = Resolution.width;
    ResolutionData.Height = Resolution.height;
    ResolutionData.Fullscreen = Fullscreen;
    ResolutionData.Apply();
  }
}
