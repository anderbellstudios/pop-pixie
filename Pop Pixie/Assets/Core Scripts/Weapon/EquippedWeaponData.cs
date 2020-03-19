using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquippedWeaponData {

  public static int CurrentWeapon {
    get {
      return GameData.Current.Fetch("current-weapon", orSetEqualTo: 0);
    }

    set {
      GameData.Current.Set("current-weapon", value);
    }
  }

}
