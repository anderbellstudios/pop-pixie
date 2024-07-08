using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDirection : MonoBehaviour {
  public static Vector2 DirectionFromWorldPoint(Vector2 point)
    => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - point;

  public static Vector2 DirectionFromScreenCenter()
    => (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2);
}
