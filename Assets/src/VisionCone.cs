using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour {
  public MeshFilter MeshFilter;
  public float AngularDistance, CentreAngle, Radius;
  public int AngleSteps;

  private Mesh Mesh;

  void Start() {
    Mesh = MeshFilter.mesh;
  }

  void LateUpdate() {
    float startAngle = CentreAngle - AngularDistance / 2f;
    float angularDistancePerStep = AngularDistance / AngleSteps;

    Vector3[] arcPoints = new Vector3[1 + AngleSteps];

    for (int step = 0; step < 1 + AngleSteps; step++) {
      float angle = startAngle + angularDistancePerStep * step;
      Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;

      RaycastHit2D hit = Physics2D.Raycast(
        transform.position,
        direction,
        Radius
      );

      arcPoints[step] = hit
        ? transform.InverseTransformPoint(hit.point)
        : direction * Radius;
    }

    DrawCone(arcPoints);
  }

  private void DrawCone(Vector3[] arcPoints) {
    Vector3[] vertices = new Vector3[1 + arcPoints.Length];
    vertices[0] = Vector3.zero;
    System.Array.Copy(arcPoints, 0, vertices, 1, arcPoints.Length);

    int triangleCount = arcPoints.Length - 1;
    int[] triangles = new int[3 * triangleCount];
    for (int i = 0; i < triangleCount; i++) {
      int offset = 3 * i;
      triangles[offset] = 0;
      triangles[offset + 1] = i + 1;
      triangles[offset + 2] = i + 2;
    }

    Mesh.Clear();
    Mesh.vertices = vertices;
    Mesh.triangles = triangles;
  }
}
