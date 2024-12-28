using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraState {
  public float Size;
  public Vector2 Position;
  public bool RelativeToPlayer = false;
  public System.Func<Vector2> GetOffset = () => Vector2.zero;

  public static CameraState FromCamera(Camera camera, bool relativeToPlayer)
    => new CameraState {
      Size = camera.orthographicSize,
      Position = relativeToPlayer
        ? AbsoluteToRelative(camera.transform.position)
        : camera.transform.position,
      RelativeToPlayer = relativeToPlayer
    };

  public static CameraState Lerp(CameraState a, CameraState b, float t)
    => new CameraState {
      Size = Mathf.Lerp(a.Size, b.Size, t),
      Position = Vector2.Lerp(a.AbsolutePosition, b.AbsolutePosition, t)
    };

  public Vector2 AbsolutePosition => RelativeToPlayer
    ? RelativeToAbsolute(OffsetPosition)
    : OffsetPosition;

  public Vector2 RelativePosition => RelativeToPlayer
    ? OffsetPosition
    : AbsoluteToRelative(OffsetPosition);

  private Vector2 OffsetPosition => Position + GetOffset();

  private static Vector2 AbsoluteToRelative(Vector2 position)
    => position - PlayerPosition;

  private static Vector2 RelativeToAbsolute(Vector2 position)
    => position + PlayerPosition;

  private static Vector2 PlayerPosition
    => PlayerGameObject.Position;
}
