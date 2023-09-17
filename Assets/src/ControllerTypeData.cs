using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTypeData {

  public static void SetControllerType(string type) {
    ConfigData.Current.Set("controller-type", type);
  }

  public static String GetControllerType() {
    return (String)ConfigData.Current.Fetch("controller-type", orSetEqualTo: "Xbox");
  }

}
