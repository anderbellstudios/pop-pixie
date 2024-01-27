using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquippedWeaponData {
  public static int CurrentWeapon {
    get {
      return (int)GameData.Current.Fetch("current-weapon", orSetEqualTo: 0);
    }

    set {
      GameData.Current.Set("current-weapon", value);
    }
  }

  public static int LastWeapon {
    get {
      return (int)GameData.Current.Fetch("last-weapon", orSetEqualTo: -1);
    }

    set {
      GameData.Current.Set("last-weapon", value);
    }
  }
}
