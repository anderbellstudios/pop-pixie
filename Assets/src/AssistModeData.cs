using System;
using System.Collections;
using System.Collections.Generic;

public class AssistModeData {
  private const decimal DEFAULT_DAMAGE_REDUCTION = 0M;

  public static decimal DamageReduction {
    get {
      return System.Convert.ToDecimal(ConfigData.Current.Fetch("assist-mode-damage-reduction", orSetEqualTo: DEFAULT_DAMAGE_REDUCTION));
    }

    set {
      ConfigData.Current.Set("assist-mode-damage-reduction", value);
    }
  }

  public static void ResetToDefaults() {
    DamageReduction = DEFAULT_DAMAGE_REDUCTION;
  }
}
