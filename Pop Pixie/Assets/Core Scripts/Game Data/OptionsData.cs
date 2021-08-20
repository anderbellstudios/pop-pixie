using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionsData {

  public static decimal MusicVolume {
    get {
      return System.Convert.ToDecimal(ConfigData.Current.Fetch("options-music-volume", orSetEqualTo: 1M));
    }

    set {
      ConfigData.Current.Set("options-music-volume", value);
    }
  }

  public static decimal SoundsVolume {
    get {
      return System.Convert.ToDecimal(ConfigData.Current.Fetch("options-sounds-volume", orSetEqualTo: 1M));
    }

    set {
      ConfigData.Current.Set("options-sounds-volume", value);
    }
  }

  public static decimal LoreWindowZoomLevel {
    get {
      return System.Convert.ToDecimal(ConfigData.Current.Fetch("options-lore-window-zoom-level", orSetEqualTo: 1M));
    }

    set {
      ConfigData.Current.Set("options-lore-window-zoom-level", value);
    }
  }

}
