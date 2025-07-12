using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionsData {
  public static decimal MusicVolume {
    get {
      return System.Convert.ToDecimal(
        ConfigData.Current.Fetch("options-music-volume", orSetEqualTo: 0.5M)
      );
    }
    set { ConfigData.Current.Set("options-music-volume", value); }
  }

  public static decimal SoundsVolume {
    get {
      return System.Convert.ToDecimal(
        ConfigData.Current.Fetch("options-sounds-volume", orSetEqualTo: 0.5M)
      );
    }
    set { ConfigData.Current.Set("options-sounds-volume", value); }
  }

  public static decimal VoiceVolume {
    get {
      return System.Convert.ToDecimal(
        ConfigData.Current.Fetch("options-voice-volume", orSetEqualTo: 0.5M)
      );
    }
    set { ConfigData.Current.Set("options-voice-volume", value); }
  }

  public static System.Guid? AudioOutput {
    get {
      string guidOrDefault = ConfigData.Current.Fetch(
        "options-audio-output",
        orSetEqualTo: "default"
      );
      if (guidOrDefault == "default")
        return null;
      return System.Guid.Parse(guidOrDefault);
    }
    set {
      string guidOrDefault = value.HasValue ? value.Value.ToString() : "default";
      ConfigData.Current.Set("options-audio-output", guidOrDefault);
    }
  }
}
