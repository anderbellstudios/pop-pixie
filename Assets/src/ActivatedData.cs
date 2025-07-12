using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivatedData {
  public static void RecordActivation(string id) {
    List<String> activatedIds = ActivatedIds;

    if (!activatedIds.Contains(id)) {
      activatedIds.Add(id);
      ActivatedIds = activatedIds;
    }
  }

  public static bool IsActivated(string id) => ActivatedIds.Contains(id);

  private static List<String> ActivatedIds {
    get {
      dynamic activatedIds = GameData.Current.Fetch(
        "activated-ids",
        orSetEqualTo: new List<String>()
      );
      return CoerceJson.To<List<String>>(activatedIds);
    }
    set { GameData.Current.Set("activated-ids", value); }
  }
}
