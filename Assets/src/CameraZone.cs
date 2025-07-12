using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {
  private static int ActiveCameraZones = 0;

  public Collider2D Collider;
  public CameraState CameraState;
  public float Duration;
  public AnimationCurve DynamicOffsetX = AnimationCurve.Constant(-1f, 1f, 0f);
  public AnimationCurve DynamicOffsetY = AnimationCurve.Constant(-1f, 1f, 0f);

  void Awake() {
    CameraState.GetOffset = GetOffset;
  }

  void OnTriggerEnter2D(Collider2D collider) {
    if (collider.tag == "Player") {
      CameraManager.Current.AnimateTo(CameraState, Duration);
      ActiveCameraZones++;
    }
  }

  void OnTriggerExit2D(Collider2D collider) {
    if (collider.tag == "Player") {
      if (--ActiveCameraZones == 0) {
        CameraManager.Current.Reset(Duration);
      }
    }
  }

  private Vector2 GetOffset() {
    Bounds bounds = Collider.bounds;
    Vector2 playerPosition = PlayerGameObject.Position;

    return new Vector2(
      DynamicOffsetX.Evaluate(
        2f * Mathf.InverseLerp(bounds.min.x, bounds.max.x, playerPosition.x) - 1f
      ),
      DynamicOffsetY.Evaluate(
        2f * Mathf.InverseLerp(bounds.min.y, bounds.max.y, playerPosition.y) - 1f
      )
    );
  }
}
