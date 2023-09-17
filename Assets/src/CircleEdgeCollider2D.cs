// Modified from https://forum.unity.com/threads/rigidbodies-inside-circle-collider.212730/

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(EdgeCollider2D))]
public class CircleEdgeCollider2D : MonoBehaviour {
  public float Width, Height = 1.0f;
  public int NumPoints = 32;

  EdgeCollider2D EdgeCollider;
  float CurrentWidth, CurrentHeight = 0.0f;

  /// <summary>
  /// Start this instance.
  /// </summary>
  void Start() {
    CreateCircle();
  }

  /// <summary>
  /// Update this instance.
  /// </summary>
  void Update() {
    // If the radius or point count has changed, update the circle
    if (NumPoints != EdgeCollider.pointCount || CurrentWidth != Width || CurrentHeight != Height) {
      CreateCircle();
    }
  }

  /// <summary>
  /// Creates the circle.
  /// </summary>
  void CreateCircle() {
    Vector2[] edgePoints = new Vector2[NumPoints + 1];
    EdgeCollider = GetComponent<EdgeCollider2D>();

    for (int loop = 0; loop <= NumPoints; loop++) {
      float angle = (Mathf.PI * 2.0f / NumPoints) * loop;
      edgePoints[loop] = new Vector2(Mathf.Sin(angle) * Width, Mathf.Cos(angle) * Height);
    }

    EdgeCollider.points = edgePoints;
    CurrentWidth = Width;
    CurrentHeight = Height;
  }
}
