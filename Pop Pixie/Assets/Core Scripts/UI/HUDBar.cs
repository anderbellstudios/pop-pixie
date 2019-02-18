using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDBar : MonoBehaviour {

  public Vector2 Position;
  public Vector2 Size;
  public Color BackgroundColour;
  public Color ForegroundColour;
  public float Progress;
  public bool Visible;

	void OnGUI () {
    if ( !Visible )
      return;

    GUI.BeginGroup(
      new Rect( 
        Screen.width  * Position.x, 
        Screen.height * Position.y, 
        Screen.width  * Size.x, 
        Screen.height * Size.y 
      )
    );

    DrawBar(BackgroundColour);
    DrawBar(ForegroundColour, Progress);

    GUI.EndGroup();
	}

  private void DrawBar (Color colour, float scaleX = 1.00f) {
    var texture = new Texture2D(1, 1);
    texture.SetPixel(0, 0, colour);
    texture.Apply();

    GUI.skin.box.normal.background = texture;

    GUI.Box(
      new Rect( 
        0, 
        0, 
        Screen.width  * Size.x * scaleX, 
        Screen.height * Size.y 
      ),
      GUIContent.none
    );
  }
}
