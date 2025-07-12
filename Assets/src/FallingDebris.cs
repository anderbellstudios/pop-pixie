using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingDebris : MonoBehaviour {
  public SpriteRenderer DebrisSpriteRenderer;
  public Image DurationIndicator,
    RadiusIndicator;
  public CanvasGroup CanvasGroup;
  public AnimationCurve CanvasScaleCurve,
    SpriteOpacityCurve,
    DistanceCurve;
  public GameObject Explosion;
  public float Distance;
  public float Duration;
  public float MaxRotationSpeed;
  public float DamageRadius,
    Damage;
  public float FadeOutDuration;
  public HitPoints CounterAttackHitPoints;
  public float CounterAttackDuration;
  public float CounterAttackDamage;

  private enum StateType {
    Falling,
    FadingOut,
    CounterAttacking,
  };

  private StateType State = StateType.Falling;
  private Stopwatch Stopwatch;
  private float RotationSpeed;

  void Start() {
    Stopwatch = new Stopwatch.PlayingTime();
    SetRadiusIndicatorRadius(DamageRadius);
    UpdateFalling();
    RotationSpeed = Random.Range(-MaxRotationSpeed, MaxRotationSpeed);
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    DebrisSpriteRenderer.transform.Rotate(0f, 0f, RotationSpeed * Time.deltaTime);

    switch (State) {
      case StateType.Falling:
        UpdateFalling();
        break;

      case StateType.FadingOut:
        UpdateFadingOut();
        break;

      case StateType.CounterAttacking:
        UpdateCounterAttacking();
        break;
    }
  }

  private void UpdateFalling() {
    float progress = Stopwatch.Progress(Duration);

    SetCanvasScale(CanvasScaleCurve.Evaluate(progress));
    SetSpritePosition(Distance * DistanceCurve.Evaluate(progress));
    SetSpriteOpacity(SpriteOpacityCurve.Evaluate(progress));

    DurationIndicator.fillAmount = progress;

    if (progress >= 1f) {
      FinishFalling();
    }
  }

  private void UpdateFadingOut() {
    float progress = Stopwatch.Progress(FadeOutDuration);
    float opacity = 1f - progress;
    SetSpriteOpacity(opacity);
    CanvasGroup.alpha = opacity;

    if (progress >= 1f) {
      Destroy(gameObject);
    }
  }

  private void UpdateCounterAttacking() {
    float progress = Stopwatch.Progress(CounterAttackDuration);

    SetSpriteWorldPosition(
      Vector3.Lerp(transform.position, CounterAttackHitPoints.transform.position, progress)
    );

    if (progress >= 1f) {
      FinishCounterAttacking();
    }
  }

  private void FinishFalling() {
    bool isCounterAttack = DamageHitPointsInRadius.Invoke(
      Damage,
      transform.position,
      DamageRadius,
      canBeCounterAttacked: true
    );

    if (isCounterAttack) {
      CanvasGroup.gameObject.SetActive(false);
      State = StateType.CounterAttacking;
      RotationSpeed *= 5f;
    } else {
      Explode();
      State = StateType.FadingOut;
      RotationSpeed = 0f;
    }

    Stopwatch.Reset();
  }

  private void FinishCounterAttacking() {
    Explode();
    CounterAttackHitPoints.Damage(CounterAttackDamage, isDestructive: true);
    State = StateType.FadingOut;
    RotationSpeed = 0f;
    Stopwatch.Reset();
  }

  private void Explode() {
    GameObject explosion = Instantiate(
      Explosion,
      DebrisSpriteRenderer.transform.position,
      Quaternion.identity
    );

    explosion.transform.localScale = 2f * DamageRadius * Vector3.one;
  }

  private void SetRadiusIndicatorRadius(float radius) {
    RadiusIndicator.transform.localScale = 2f * radius * Vector3.one;
  }

  private void SetCanvasScale(float scale) {
    CanvasGroup.transform.localScale = scale * Vector3.one;
  }

  private void SetSpritePosition(float y) {
    DebrisSpriteRenderer.transform.localPosition = y * Vector3.up;
  }

  private void SetSpriteWorldPosition(Vector3 position) {
    DebrisSpriteRenderer.transform.position = position;
  }

  private void SetSpriteOpacity(float opacity) {
    DebrisSpriteRenderer.color = new Color(1f, 1f, 1f, opacity);
  }
}
