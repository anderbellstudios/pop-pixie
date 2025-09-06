using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDirection : MonoBehaviour {
  public static Vector2 DirectionFromWorldPoint(Vector2 point) =>
    (Vector2)Camera.main.ScreenToWorldPoint(WrappedInput.MousePosition) - point;

  public static Vector2 DirectionFromScreenCenter() =>
    (Vector2)WrappedInput.MousePosition - new Vector2(Screen.width / 2, Screen.height / 2);
}
