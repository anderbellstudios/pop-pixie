using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePadButtonData {

  private static readonly string keyPrefix = "game-pad-buton-v1.1-";

  public static GamePadButton GetButton( string buttonName ) {
    return ConfigData.Current.Fetch( keyPrefix + buttonName );
  }

  public static void SetButton( string buttonName, GamePadButton button ) {
    ConfigData.Current.Set( keyPrefix + buttonName, button );
  }

}
