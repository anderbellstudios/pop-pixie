using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionsData {

  public static decimal MusicVolume {
    get {
      return GameData.Fetch("options-music-volume", orSetEqualTo: 1M);
    }

    set {
      GameData.Set("options-music-volume", value);
    }
  }

}
