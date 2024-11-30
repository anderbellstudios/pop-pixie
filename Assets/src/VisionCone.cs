using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisionCone : MonoBehaviour {
  public bool Static;
  public MeshFilter MeshFilter;
  public MeshRenderer MeshRenderer;
  public SpriteRenderer VisibilityRange;
  public LayerMask BlockingMask;
  public float Width;
  public int AngleSteps;
  public float Radius;
  public float BlindSpotRadius;
  public float DetectCornerThreshold;
  public int DetectCornerIterations;
  public float DetectPlayerMinDuration;
  public float DetectPlayerMaxDistanceLeniency;
  public UnityEvent OnDetectPlayer;

  private Mesh Mesh;
  private Stopwatch SeesPlayerStopwatch;
  private LayerMask BlockingAndPlayerMask;
  private bool FirstRender;

  private enum HitResultType { None, Miss, Hit };

  public void SetColor(Color color) {
    MeshRenderer.material.color = color;
  }

  public void SetYellow() => SetColor(Color.yellow);
  public void SetRed() => SetColor(Color.red);

  void Start() {
    Mesh = MeshFilter.mesh;
    MeshRenderer.material.SetFloat("_Radius", Radius);
    MeshRenderer.material.SetFloat("_BlindRadius", BlindSpotRadius);
    BlockingAndPlayerMask = BlockingMask |
      LayerMask.GetMask("Player") |
      LayerMask.GetMask("PlayerRolling");
    VisibilityRange.gameObject.transform.localScale = Vector3.one * Radius * 2;
    FirstRender = true;
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
    /**
     * Optimisation: Do not perform any raycasts if the vision cone cannot
     * possibly appear on screen.
     */
    if (!VisibilityRange.isVisible)
      return;

    /**
     * Optimisation: After the first render, do not perform raycasts while
     * the game is paused, or at all if the vision cone is static, since the
     * result will be the same.
     */
    if (!FirstRender && (Static || !StateManager.Playing))
      return;

    FirstRender = false;

    List<Vector3> arcPoints = new List<Vector3>();

    ScanArc(
      startAngle: -Width / 2f,
      angularDistance: Width,
      steps: AngleSteps,
      outList: arcPoints,
      onDetectCorner: DetectCornerIterations > 0
        ? (angle) => ScanArc(
          startAngle: angle,
          angularDistance: Width / AngleSteps,
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
        transform.position,
        transform.TransformDirection(direction),
        Radius,
        BlockingMask
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
    float relativeAngle = Vector3.Angle(Vector3.right, point);
    float distance = point.magnitude;

    return relativeAngle < Width / 2 &&
      distance >= BlindSpotRadius &&
      distance <= Radius - DetectPlayerMaxDistanceLeniency;
  }

  private bool RaycastHitsPlayer(Vector2 direction) {
    RaycastHit2D hit = Physics2D.Raycast(
      transform.position,
      transform.TransformDirection(direction),
      Radius,
      BlockingAndPlayerMask
    );

    return hit && hit.collider.tag == "Player";
  }
}
