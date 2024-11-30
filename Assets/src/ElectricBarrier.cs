using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBarrier : MonoBehaviour {
  [System.Serializable]
  public struct WidthAndOpacity {
    public float Width, Opacity;
  }

  public Transform Terminal1, Terminal2;
  public GameObject LineRendererGameObject;
  public int Points;
  public float Amplitude;
  public int Passes = 1;
  public Color Color;
  public List<WidthAndOpacity> LineRendererData;

  private LineRenderer[,] LineRenderersByPass;

  void Start() {
    LineRenderersByPass = new LineRenderer[Passes, LineRendererData.Count];

    /**
     * For each pass, provision a number of LineRenderers equal to the length
     * of the LineRendererData list.
     */
    for (int pass = 0; pass < Passes; pass++) {
      for (int i = 0; i < LineRendererData.Count; i++) {
        WidthAndOpacity data = LineRendererData[i];

        GameObject newGameObject = Instantiate(LineRendererGameObject, transform);
        LineRenderer lineRenderer = newGameObject.GetComponent<LineRenderer>();

        Color color = new Color(Color.r, Color.g, Color.b, data.Opacity);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.startWidth = data.Width;
        lineRenderer.endWidth = data.Width;

        lineRenderer.positionCount = Points + 2;

        LineRenderersByPass[pass, i] = lineRenderer;
      }
    }

    // Deactivate the template game object
    LineRendererGameObject.SetActive(false);

    Render();
  }

  void Update() {
    if (!StateManager.Enabled(StateFeatures.BackgroundAnimations))
      return;

    Render();
  }

  private void Render() {
    Matrix4x4 matrix = GetMatrix();

    System.Func<Vector3, Vector3> transformPoint = point => {
      Vector4 pointWithW = point;
      pointWithW.w = 1f;
      return matrix * pointWithW;
    };

    System.Action<int, int, Vector3> setPoint = (pass, pointIndex, point) => {
      for (
        int lineRendererIndex = 0;
        lineRendererIndex < LineRendererData.Count;
        lineRendererIndex++
      ) {
        LineRenderer lineRenderer = LineRenderersByPass[pass, lineRendererIndex];
        lineRenderer.SetPosition(pointIndex, transformPoint(point));
      }
    };

    /**
     * Divide the horizontal distance into equal chunks with Points vertices
     * between the start and end points.
     */
    float xDistancePerPoint = 1f / (Points + 1);

    for (int pass = 0; pass < Passes; pass++) {
      // Start point
      setPoint(pass, 0, Vector3.zero);

      /**
       * Since the 0th point of the pass has already been handled, skip to the
       * 1th point, which is the first of the Points vertices between the start
       * and end points. Stop just before the end point of the current pass.
       */
      for (int i = 1; i <= Points; i++) {
        Vector3 point = new Vector3(
          xDistancePerPoint * i,
          Random.Range(-Amplitude / 2f, Amplitude / 2f),
          Random.Range(-Amplitude / 2f, Amplitude / 2f)
        );

        setPoint(pass, i, point);
      }

      // End point
      setPoint(pass, Points + 1, Vector3.right);
    }
  }

  /**
   * Inverse of the matrix that:
   *   Translates the first terminal to the origin, rotates such that both
   *   terminals lie on the x-axis, and scales such that the second terminal is
   *   at (1, 0). The scale in the perpendicular axis (now the y-axis) remains
   *   the same.
   */
  private Matrix4x4 GetMatrix() {
    Vector3 firstToSecond = Terminal2.position - Terminal1.position;

    return Matrix4x4.Translate(Terminal1.position) *
      Matrix4x4.Rotate(
        Quaternion.FromToRotation(Vector3.right, firstToSecond)
      ) *
      Matrix4x4.Scale(
        new Vector3(firstToSecond.magnitude, 1f, 1f)
      );
  }
}
