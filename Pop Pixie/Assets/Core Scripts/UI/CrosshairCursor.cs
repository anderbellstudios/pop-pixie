using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour {

  public Texture2D CrosshairTexture;

	// Update is called once per frame
	void Update () {
    Cursor.SetCursor(
      CrosshairTexture, 
      Vector2.zero, 
      CursorMode.Auto
    );
	}
}
