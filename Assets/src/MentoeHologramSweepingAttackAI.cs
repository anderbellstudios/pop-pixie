using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MentoeHologramSweepingAttackAI : AMovementEnemyAI {
  public float SafeAngle;
  public float BeforeLaserDuration,
    LaserDuration,
    LaserBeamLength;
  public AnimationCurve DangerZoneExpandCurve;
  public Transform DangerZoneTransform;
  public Image DangerZoneImage;
  public LineRenderer LineRenderer;
  public UnityEvent OnBeginLaser;
  public DamageMultiHitPointEntity DamageBoss;

  private Stopwatch DangerZoneExpandStopwatch,
    LaserStopwatch;
  private float CurrentDangerZoneExpandDuration,
    CurrentLaserDuration;
  private float StartAngle,
    PreviousAngle;

  protected override void OnActivate() {
    DangerZoneExpandStopwatch = new Stopwatch.PlayingTime();
    LaserStopwatch = null;

    CurrentDangerZoneExpandDuration = BeforeLaserDuration + LaserDuration;
    CurrentLaserDuration = LaserDuration;

    StartAngle = Random.Range(0, 360);
    PreviousAngle = 0;

    DangerZoneImage.enabled = true;
    DangerZoneImage.fillAmount = 1 - (SafeAngle / 360);
    DangerZoneTransform.localRotation = Quaternion.Euler(0, 0, StartAngle - SafeAngle);
    DangerZoneTransform.localScale = Vector3.zero;

    Helper.SetTimeout(
      () => {
        LaserStopwatch = new Stopwatch.PlayingTime();
        OnBeginLaser.Invoke();
      },
      BeforeLaserDuration
    );
  }

  protected override void OnDeactivate() {
    LineRenderer.enabled = false;
    DangerZoneImage.enabled = false;
  }

  protected override void WhileActive() {
    float dangerZoneProgress = DangerZoneExpandStopwatch.Progress(CurrentDangerZoneExpandDuration);
    DangerZoneTransform.localScale =
      DangerZoneExpandCurve.Evaluate(dangerZoneProgress) * Vector3.one;

    if (LaserStopwatch == null)
      return;

    float laserProgress = LaserStopwatch.Progress(CurrentLaserDuration);

    if (laserProgress >= 1f) {
      OnFinish();
      return;
    }

    float angle = Mathf.Lerp(0, 360 - SafeAngle, laserProgress);
    Vector3 direction = Quaternion.Euler(0, 0, StartAngle + angle) * Vector3.right;

    LineRenderer.enabled = true;
    LineRenderer.SetPosition(0, transform.position);
    LineRenderer.SetPosition(1, transform.position + LaserBeamLength * direction);

    Vector3 playerDirection = Helper.DirectionToPlayer;

    float playerAngle =
      ((Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg) - StartAngle + 360)
      % 360;

    if (PreviousAngle <= playerAngle && playerAngle <= angle) {
      bool isCounterAttack = Helper.DamagePlayer(1, true);
      if (isCounterAttack)
        DamageBoss.Damage(150);
    }

    PreviousAngle = angle;
  }
}
