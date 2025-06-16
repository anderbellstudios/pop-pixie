using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static CameraManager Current;

  public Camera Camera;
  public bool StartsRelativeToPlayer = true;

  private CameraState DefaultState, PreviousState, TargetState;
  private float ZPosition;
  private Stopwatch AnimationStopwatch;
  private float AnimationDuration;
  private System.Action OnFinishOrInterrupt;

  void Awake() {
    if (SingletonInstance) {
      if (Current != null) {
        throw new System.Exception("Cannot have multiple singleton instances of CameraManager");
      }
      Current = this;
    }
  }

  void Start() {
    DefaultState = PreviousState = TargetState = CameraState.FromCamera(
      Camera,
      StartsRelativeToPlayer && PlayerGameObject.Current != null
    );

    ZPosition = Camera.transform.position.z;
  }

  void Update() {
    Apply(CurrentState);

    if (Animating && AnimationProgress >= 1f) {
      FinishedOrInterrupted();
      PreviousState = TargetState;
      AnimationStopwatch = null;
    }
  }

  public void AnimateTo(
    CameraState state,
    float duration,
    System.Action onFinishOrInterrupt = null
  ) {
    FinishedOrInterrupted();
    PreviousState = CurrentState;
    TargetState = state;
    AnimationDuration = duration;
    AnimationStopwatch = new Stopwatch.BaseTime();
    OnFinishOrInterrupt = onFinishOrInterrupt;
  }

  public void Reset(float duration) => AnimateTo(DefaultState, duration);

  private void Apply(CameraState state) {
    Camera.orthographicSize = state.Size;

    Vector2 position = state.AbsolutePosition;
    Camera.transform.position = new Vector3(
      position.x,
      position.y,
      ZPosition
    );
  }

  private void FinishedOrInterrupted() {
    if (OnFinishOrInterrupt != null) {
      OnFinishOrInterrupt();
      OnFinishOrInterrupt = null;
    }
  }

  private CameraState CurrentState => Animating
    ? CameraState.Lerp(PreviousState, TargetState, SmoothAnimationProgress)
    : TargetState;

  private bool Animating => AnimationStopwatch != null;
  private float SmoothAnimationProgress => Mathf.SmoothStep(0f, 1f, AnimationProgress);
  private float AnimationProgress => AnimationStopwatch.Progress(AnimationDuration);
}
