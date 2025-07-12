using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResolutionData {
  public static int Width {
    set { ConfigData.Current.Set("resolution-width", value); }
    get {
      return (int)
        ConfigData.Current.Fetch("resolution-width", orSetEqualTo: DefaultResolution().width);
    }
  }

  public static int Height {
    set { ConfigData.Current.Set("resolution-height", value); }
    get {
      return (int)
        ConfigData.Current.Fetch("resolution-height", orSetEqualTo: DefaultResolution().height);
    }
  }

  public static bool Fullscreen {
    set { ConfigData.Current.Set("resolution-fullscreen", value); }
    get { return (bool)ConfigData.Current.Fetch("resolution-fullscreen", orSetEqualTo: false); }
  }

  public static void Apply() {
    EnhancedDataCollection.LogIfEnabled(() =>
      $"Resolution: {Width}x{Height}, Fullscreen: {Fullscreen}"
    );

#if UNITY_EDITOR
    Debug.Log($"Setting resolution: {Width}x{Height}, Fullscreen: {Fullscreen}");
#else
    Screen.SetResolution(Width, Height, Fullscreen);
#endif
  }

  private static Resolution DefaultResolution() {
    List<Resolution> validResolutions = Screen
      .resolutions.Where(resolution =>
        resolution.width <= Display.main.systemWidth
        && resolution.height <= Display.main.systemHeight
      )
      .ToList();

    // Should never happen
    if (validResolutions.Count == 0) {
      return Screen.resolutions[0];
    }

    // Default to the median supported resolution
    return validResolutions[validResolutions.Count / 2];
  }
}
