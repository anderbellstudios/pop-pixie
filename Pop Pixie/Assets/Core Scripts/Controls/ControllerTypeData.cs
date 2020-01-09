using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTypeData {

  public static void SetType( string type ) {
    ConfigData.Current.Set( "controller-type", type );
  }

  public static String GetType() {
    return ConfigData.Current.Fetch( "controller-type" );
  }

}
