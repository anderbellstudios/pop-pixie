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
  public Color Color;
  public List<WidthAndOpacity> LineRendererData;

  private List<LineRenderer> LineRenderers = new();

  void Start() {
    LineRendererData.ForEach(data => {
      GameObject newGameObject = Instantiate(LineRendererGameObject, transform);
      LineRenderer lineRenderer = newGameObject.GetComponent<LineRenderer>();
      Color color = new Color(Color.r, Color.g, Color.b, data.Opacity);
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
      lineRenderer.startWidth = data.Width;
      lineRenderer.endWidth = data.Width;
      lineRenderer.positionCount = Points + 2;
      LineRenderers.Add(lineRenderer);
    });

    LineRendererGameObject.SetActive(false);

    Render();
  }

  void Update() {
    if (!StateManager.Playing)
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

    System.Action<int, Vector3> setPoint = (index, point) => {
      LineRenderers.ForEach(lineRenderer => {
        lineRenderer.SetPosition(index, transformPoint(point));
      });
    };

    float xDistancePerPoint = 1f / (Points + 1);

    setPoint(0, Vector3.zero);
    setPoint(Points + 1, Vector3.right);

    for (int i = 1; i < Points + 1; i++) {
      Vector3 point = new Vector3(
        xDistancePerPoint * i,
        Random.Range(-Amplitude / 2f, Amplitude / 2f),
        1f
      );

      setPoint(i, point);
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
