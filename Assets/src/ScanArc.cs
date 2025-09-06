using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaycastFn = System.Func<float, ScanArc.RaycastResult>;

public class ScanArc {
  public class RaycastResult {
    public GameObject GameObject = null;
    public Vector3 Point;

    public static RaycastResult Hit(GameObject gameObject, Vector3 point) =>
      new RaycastResult() { GameObject = gameObject, Point = point };

    public static RaycastResult Miss(float maxDistance, Vector3 direction) =>
      new RaycastResult() { Point = direction.normalized * maxDistance };

    public bool IsMiss => GameObject == null;
    public bool IsHit => !IsMiss;
    public float Distance => Point.magnitude;
  }

  private RaycastFn Raycast;
  private int AngleSteps;
  private float DetectCornerThreshold;
  private int DetectCornerIterations;

  private RaycastResult PreviousPreviousResult,
    PreviousResult;

  public ScanArc(
    int angleSteps,
    float detectCornerThreshold,
    int detectCornerIterations,
    RaycastFn raycast
  ) {
    AngleSteps = angleSteps;
    DetectCornerThreshold = detectCornerThreshold;
    DetectCornerIterations = detectCornerIterations;
    Raycast = raycast;
  }

  public Vector3[] Scan(float startAngle, float endAngle) {
    List<Vector3> arcPoints = new List<Vector3>();

    PreviousPreviousResult = null;
    PreviousResult = null;

    Pass(
      startAngle: startAngle,
      endAngle: endAngle,
      steps: AngleSteps,
      outList: arcPoints,
      onDetectCorner: DetectCornerIterations > 0
        ? (previousAngle, angle) =>
          Pass(
            startAngle: previousAngle,
            endAngle: angle,
            steps: DetectCornerIterations,
            skipFirstAndLast: true,
            outList: arcPoints
          )
        : null
    );

    return arcPoints.ToArray();
  }

  private void Pass(
    float startAngle,
    float endAngle,
    float steps,
    List<Vector3> outList,
    bool skipFirstAndLast = false,
    System.Action<float, float> onDetectCorner = null
  ) {
    float angularDistancePerStep = (endAngle - startAngle) / steps;

    for (int step = 0; step < steps + 1; step++) {
      if (skipFirstAndLast && (step == 0 || step == steps))
        continue;

      float angle = startAngle + angularDistancePerStep * step;
      RaycastResult result = Raycast(angle);

      if (onDetectCorner != null && ShouldDetectCorner(result)) {
        onDetectCorner(angle - angularDistancePerStep, angle);
      }

      outList.Add(result.Point);

      PreviousPreviousResult = PreviousResult;
      PreviousResult = result;
    }
  }

  private bool ShouldDetectCorner(RaycastResult result) {
    /**
     * Never detect corner on the first angle, since there's no previous
     * angle to scan from.
     */
    if (PreviousResult == null)
      return false;

    /**
     * Detect corner on Hit -> Miss. This is the only case where we detect on a
     * Miss.
     */
    if (result.IsMiss)
      return PreviousResult.IsHit;

    // Detect corner on Miss -> Hit
    if (PreviousResult.IsMiss)
      return true;

    /**
     * Detect corner on non-Hit -> Hit -> Hit, since there isn't enough
     * information to check if the previous two hits are part of the same line.
     */
    if (PreviousPreviousResult == null || PreviousPreviousResult.IsMiss)
      return true;

    // Detect corner when the last two hits were on different objects
    if (result.GameObject != PreviousResult.GameObject)
      return true;

    // All three are hits, so check if they form a straight line
    return !PointsFormLine(PreviousPreviousResult.Point, PreviousResult.Point, result.Point);
  }

  private bool PointsFormLine(Vector2 a, Vector2 b, Vector2 c) {
    // Handle vertical lines as a special case
    if (
      Mathf.Abs(a.x - b.x) <= DetectCornerThreshold
      && Mathf.Abs(a.x - c.x) <= DetectCornerThreshold
    )
      return true;

    // Line between A and B
    float gradient = (b.y - a.y) / (b.x - a.x);
    float yIntersect = a.y - gradient * a.x;

    // Expected y for C
    float expectedY = gradient * c.x + yIntersect;

    float delta = Mathf.Abs(c.y - expectedY);
    return delta <= DetectCornerThreshold;
  }
}
