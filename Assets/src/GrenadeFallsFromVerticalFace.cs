using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrenadeFallsFromVerticalFace : MonoBehaviour {
  public BulletLayer BulletLayer;
  public Rigidbody2D Rigidbody;
  public GrenadeWaitingBeforeThrow GrenadeWaitingBeforeThrow;
  public float Gravity;

  private bool Falling = false;

  void Update() {
    if (!StateManager.Playing || GrenadeWaitingBeforeThrow.Waiting)
      return;

    if (!TouchingVerticalFace && Falling) {
      StopFalling();
    }

    if (TouchingVerticalFace && !Falling) {
      MaybeStartFalling();
    }

    if (Falling) {
      Rigidbody.velocity += Vector2.down * Gravity * Time.deltaTime;
    }
  }

  private void MaybeStartFalling() {
    if (WillLandOnVerticalFace) {
      Falling = true;
    }
  }

  private void StopFalling() {
    Falling = false;
    Rigidbody.velocity = Vector3.zero;
  }

  private bool WillLandOnVerticalFace => IsOnVerticalFace(RestingPosition);
  private Vector3 RestingPosition => DragUtils.RestingPosition(Rigidbody);

  private bool IsOnVerticalFace(Vector3 point) => VerticalFace.OverlapPoint(point);

  private bool TouchingVerticalFace => TouchingVerticalFaces.Count > 0;
  private Collider2D VerticalFace => TouchingVerticalFaces.FirstOrDefault();

  public List<Collider2D> TouchingVerticalFaces => BulletLayer.TouchingVerticalFaces;
}
