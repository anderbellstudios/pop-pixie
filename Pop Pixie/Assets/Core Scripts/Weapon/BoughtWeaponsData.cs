using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoughtWeaponsData {

  public static bool IsBought(string weaponId) {
    return (bool) GameData.Current.Fetch(
      "bought-" + weaponId,
      orSetEqualTo: false
    );
  }

  public static void SetBought(string weaponId, bool bought) {
    GameData.Current.Set("bought-" + weaponId, bought);
  }

}
