using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour {

  public Texture2D CrosshairTexture;

	// Update is called once per frame
	void Update () {
    Cursor.SetCursor(
      CrosshairTexture, 
      new Vector2( 16, 16 ),
      CursorMode.Auto
    );
	}
}
