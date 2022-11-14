using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionData {
  public static int Width {
    set {
      ConfigData.Current.Set("resolution-width", value);
    }

    get {
      return (int) ConfigData.Current.Fetch("resolution-width", orSetEqualTo: (int) (Display.main.renderingWidth * 0.75f));
    }
  }

  public static int Height {
    set {
      ConfigData.Current.Set("resolution-height", value);
    }

    get {
      return (int) ConfigData.Current.Fetch("resolution-height", orSetEqualTo: (int) (Display.main.renderingHeight * 0.75f));
    }
  }

  public static bool Fullscreen {
    set {
      ConfigData.Current.Set("resolution-fullscreen", value);
    }

    get {
      return (bool) ConfigData.Current.Fetch("resolution-fullscreen", orSetEqualTo: false);
    }
  }

  public static void Apply() {
    EnhancedDataCollection.LogIfEnabled(() => $"Resolution: {Width}x{Height}, Fullscreen: {Fullscreen}");

#if UNITY_EDITOR
    Debug.Log($"Setting resolution: {Width}x{Height}, Fullscreen: {Fullscreen}");
#else
    Screen.SetResolution(Width, Height, Fullscreen);
#endif
  }
}
