using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CalibrationButton {

  public string Name;
  public Sprite PSIcon;
  public Sprite XboxIcon;

  public Sprite Icon( string type ) {
    switch ( type ) {
      case "PS":
        return PSIcon;
        break;

      case "Xbox":
        return XboxIcon;
        break;

      default:
        return null;
    }
  }

}
