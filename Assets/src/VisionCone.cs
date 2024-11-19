using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisionCone : MonoBehaviour {
  public MeshFilter MeshFilter;
  public MeshRenderer MeshRenderer;
  public float AngularDistance;
  public int AngleSteps;
  public float CentreAngle;
  public float Radius;
  public float BlindSpotRadius;
  public float DetectCornerThreshold;
  public int DetectCornerIterations;
  public float DetectPlayerMinDuration;
  public float DetectPlayerMaxDistanceLeniency;
  public UnityEvent OnDetectPlayer;

  private Mesh Mesh;
  private Stopwatch SeesPlayerStopwatch;

  private enum HitResultType { None, Miss, Hit };

  void Start() {
    Mesh = MeshFilter.mesh;
    MeshRenderer.material.SetFloat("_Radius", Radius);
    MeshRenderer.material.SetFloat("_BlindRadius", BlindSpotRadius);
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    // We only care about the centre of the player, relative to our own origin
    Vector2 playerPosition = transform.InverseTransformPoint(
      PlayerGameObject.Current.transform.position
    );

    bool seesPlayer = PointIsInsideCone(playerPosition) &&
      RaycastHitsPlayer(playerPosition.normalized);

    if (seesPlayer) {
      if (SeesPlayerStopwatch == null) {
        SeesPlayerStopwatch = new Stopwatch.PlayingTime();
      }

      if (SeesPlayerStopwatch.Time() >= DetectPlayerMinDuration) {
        OnDetectPlayer.Invoke();
      }
    } else {
      SeesPlayerStopwatch = null;
    }
  }

  void LateUpdate() {
    List<Vector3> arcPoints = new List<Vector3>();

    ScanArc(
      startAngle: CentreAngle - AngularDistance / 2f,
      angularDistance: AngularDistance,
      steps: AngleSteps,
      outList: arcPoints,
      onDetectCorner: DetectCornerIterations > 0
        ? (angle) => ScanArc(
          startAngle: angle,
          angularDistance: AngularDistance / AngleSteps,
          steps: DetectCornerIterations,
          skipFirstAndLast: true,
          outList: arcPoints
        )
        : null
    );

    DrawCone(arcPoints.ToArray());
  }

  private void ScanArc(
    float startAngle,
    float angularDistance,
    float steps,
    List<Vector3> outList,
    bool skipFirstAndLast = false,
    System.Action<float> onDetectCorner = null
  ) {
    float angularDistancePerStep = angularDistance / steps;

    RaycastHit2D? previousPreviousHit = null;
    RaycastHit2D? previousHit = null;

    for (int step = 0; step < steps + 1; step++) {
      if (skipFirstAndLast && (step == 0 || step == steps))
        continue;

      float angle = startAngle + angularDistancePerStep * step;
      Vector3 direction = DirectionForAngle(angle);

      RaycastHit2D hit = Physics2D.Raycast(
        transform.position + direction * BlindSpotRadius,
        direction,
        Radius
      );

      if (
        onDetectCorner != null &&
        ShouldDetectCorner(previousPreviousHit, previousHit, hit)
      ) {
        onDetectCorner(angle - angularDistancePerStep);
      }

      Vector3 point = hit
          ? transform.InverseTransformPoint(hit.point)
          : direction * Radius;

      outList.Add(point);

      previousPreviousHit = previousHit;
      previousHit = hit;
    }
  }

  private bool ShouldDetectCorner(
    RaycastHit2D? previousPreviousHit,
    RaycastHit2D? previousHit,
    RaycastHit2D hit
  ) {
    HitResultType currentResultType = ToHitResultType(hit);
    HitResultType previousResultType = ToHitResultType(previousHit);
    HitResultType previousPreviousResultType = ToHitResultType(previousPreviousHit);

    /**
     * Never detect corner on the first angle, since there's no previous
     * angle to scan from.
     */
    if (previousResultType == HitResultType.None)
      return false;

    /**
     * Detect corner on Hit -> Miss. This is the only case where we detect on a
     * Miss.
     */
    if (currentResultType == HitResultType.Miss) {
      return previousResultType == HitResultType.Hit;
    }

    // Detect corner on Miss -> Hit
    if (previousResultType == HitResultType.Miss)
      return true;

    /**
     * Detect corner on non-Hit -> Hit -> Hit, since there isn't enough
     * information to check if the previous two hits are part of the same line.
     */
    if (previousPreviousResultType != HitResultType.Hit)
      return true;

    // All three are hits, so check if they form a straight line
    return !PointsFormLine(
      previousPreviousHit.Value.point,
      previousHit.Value.point,
      hit.point
    );
  }

  private HitResultType ToHitResultType(RaycastHit2D? hit) => hit.HasValue
    ? ToHitResultType(hit.Value)
    : HitResultType.None;

  private HitResultType ToHitResultType(RaycastHit2D hit) => hit
    ? HitResultType.Hit
    : HitResultType.Miss;

  private bool PointsFormLine(Vector2 a, Vector2 b, Vector2 c) {
    // Handle vertical lines as a special case
    if (
      Mathf.Abs(a.x - b.x) <= DetectCornerThreshold &&
      Mathf.Abs(a.x - c.x) <= DetectCornerThreshold
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

  private void DrawCone(Vector3[] arcPoints) {
    Vector3[] vertices = new Vector3[1 + arcPoints.Length];
    vertices[0] = Vector3.zero;
    arcPoints.CopyTo(vertices, 1);

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

  private Vector3 DirectionForAngle(float angle) =>
    Quaternion.Euler(0, 0, angle) * Vector3.right;

  private bool PointIsInsideCone(Vector2 point) {
    float relativeAngle = Vector3.Angle(DirectionForAngle(CentreAngle), point);
    float distance = point.magnitude;

    return relativeAngle < AngularDistance / 2 &&
      distance >= BlindSpotRadius &&
      distance <= Radius - DetectPlayerMaxDistanceLeniency;
  }

  private bool RaycastHitsPlayer(Vector2 direction) {
    RaycastHit2D hit = Physics2D.Raycast(
      transform.position,
      direction,
      Radius
    );

    return hit && hit.collider.tag == "Player";
  }
}
