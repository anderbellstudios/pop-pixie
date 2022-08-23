using System;
using System.Collections;
using System.Collections.Generic;

public class AssistModeData {
  public static decimal DamageReduction {
    get {
      return System.Convert.ToDecimal(ConfigData.Current.Fetch("assist-mode-damage-reduction", orSetEqualTo: 0M));
    }

    set {
      ConfigData.Current.Set("assist-mode-damage-reduction", value);
    }
  }
}
