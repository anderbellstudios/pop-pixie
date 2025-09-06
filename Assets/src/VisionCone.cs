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

  public void SetColor(Color color) {
    MeshRenderer.material.color = color;
  }

  public void SetYellow() => SetColor(Color.yellow);

  public void SetRed() => SetColor(Color.red);

  void Start() {
    Mesh = MeshFilter.mesh;
    MeshRenderer.material.SetFloat("_Radius", Radius);
    MeshRenderer.material.SetFloat("_BlindRadius", BlindSpotRadius);
    BlockingAndPlayerMask = BlockingMask | CollisionMask.PlayerMask;
    VisibilityRange.gameObject.transform.localScale = Vector3.one * Radius * 2;
    FirstRender = true;
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    // We only care about the centre of the player, relative to our own origin
    Vector2 playerPosition = transform.InverseTransformPoint(PlayerGameObject.Position);

    bool seesPlayer =
      PointIsInsideCone(playerPosition) && RaycastHitsPlayer(playerPosition.normalized);

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

    ScanArc scanArc = new ScanArc(
      angleSteps: AngleSteps,
      detectCornerThreshold: DetectCornerThreshold,
      detectCornerIterations: DetectCornerIterations,
      raycast: RaycastForScan
    );

    DrawCone(scanArc.Scan(-Width / 2f, Width / 2f));
  }

  private ScanArc.RaycastResult RaycastForScan(float angle) {
    Vector3 direction = DirectionForAngle(angle);

    RaycastHit2D hit = Physics2D.Raycast(
      transform.position,
      transform.TransformDirection(direction),
      Radius,
      BlockingMask
    );

#if UNITY_EDITOR
    Debug.DrawLine(
      transform.position,
      transform.position + transform.TransformDirection(direction) * (hit ? hit.distance : Radius)
    );
#endif

    if (hit) {
      return ScanArc.RaycastResult.Hit(
        hit.collider.gameObject,
        transform.InverseTransformPoint(hit.point)
      );
    }

    return ScanArc.RaycastResult.Miss(Radius, direction);
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

  private Vector3 DirectionForAngle(float angle) => Quaternion.Euler(0, 0, angle) * Vector3.right;

  private bool PointIsInsideCone(Vector2 point) {
    float relativeAngle = Vector3.Angle(Vector3.right, point);
    float distance = point.magnitude;

    return relativeAngle < Width / 2
      && distance >= BlindSpotRadius
      && distance <= Radius - DetectPlayerMaxDistanceLeniency;
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
