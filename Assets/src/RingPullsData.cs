using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingPullsData {

  public static bool ShouldPulse = false;

  public static int Amount() {
    return (int) GameData.Current.Fetch(
      "ring-pulls", 
      orSetEqualTo: Debug.isDebugBuild ? 500 : 0
    );
  }

  public static void SetAmount(int amount) {
    GameData.Current.Set("ring-pulls", amount);
  }

  public static void Increment() {
    Modify(1);
  }

  public static void Modify(int delta) {
    SetAmount( Amount() + delta );
  }

}
