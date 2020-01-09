using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePadAxisData {

  public static string GetInput( string axis ) {
    return GameData.Current.Fetch(
      "game-pad-axis-" + axis + "-input"
    );
  }

  public static int GetSign( string axis ) {
    return GameData.Current.Fetch(
      "game-pad-axis-" + axis + "-sign",
      orSetEqualTo: 0
    );
  }

  public static void SetAxis( string axis, string input, int sign ) {
    GameData.Current.Set( "game-pad-axis-" + axis + "-input", input );
    GameData.Current.Set( "game-pad-axis-" + axis + "-sign", sign );
  }

}
