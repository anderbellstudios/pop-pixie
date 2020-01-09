using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePadButtonData {

  public static KeyCode? GetKeyCode( string button ) {
    return GameData.Current.Fetch( "game-pad-button-" + button );
  }

  public static void SetKeyCode( string button, KeyCode keyCode ) {
    GameData.Current.Set( "game-pad-button-" + button, keyCode );
  }

}
