using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MentoeHologramSweepingAttackAI : AEnemyAI {
  public float SafeAngle;
  public float BeforeLaserDuration, LaserDuration, LaserBeamLength;
  public AnimationCurve DangerZoneExpandCurve;
  public Transform DangerZoneTransform;
  public Image DangerZoneImage;
  public LineRenderer LineRenderer;
  public UnityEvent OnBeginLaser;
  public DamageMultiHitPointEntity DamageBoss;

  public AEnemyAI WhenFinished;

  private Stopwatch DangerZoneExpandStopwatch, LaserStopwatch;
  private float CurrentDangerZoneExpandDuration, CurrentLaserDuration;
  private float StartAngle, PreviousAngle;

  public override void ControlGained() {
    DangerZoneExpandStopwatch = new Stopwatch.PlayingTime();
    LaserStopwatch = null;

    CurrentDangerZoneExpandDuration = BeforeLaserDuration + LaserDuration;
    CurrentLaserDuration = LaserDuration;

    StartAngle = Random.Range(0, 360);
    PreviousAngle = 0;

    DangerZoneImage.enabled = true;
    DangerZoneImage.fillAmount = 1 - (SafeAngle / 360);
    DangerZoneTransform.localRotation = Quaternion.Euler(0, 0, StartAngle - SafeAngle);

    SetTimeout(() => {
      LaserStopwatch = new Stopwatch.PlayingTime();
      LineRenderer.enabled = true;
      OnBeginLaser.Invoke();
    }, BeforeLaserDuration);
  }

  public override void WhileInControl() {
    float dangerZoneProgress = DangerZoneExpandStopwatch.Progress(CurrentDangerZoneExpandDuration);
    DangerZoneTransform.localScale = DangerZoneExpandCurve.Evaluate(dangerZoneProgress) * Vector3.one;

    if (LaserStopwatch == null)
      return;

    float laserProgress = LaserStopwatch.Progress(CurrentLaserDuration);

    if (laserProgress >= 1f) {
      RelinquishControlTo(WhenFinished);
      return;
    }

    float angle = Mathf.Lerp(0, 360 - SafeAngle, laserProgress);
    Vector3 direction = Quaternion.Euler(0, 0, StartAngle + angle) * Vector3.right;

    LineRenderer.SetPosition(0, transform.position);
    LineRenderer.SetPosition(1, transform.position + LaserBeamLength * direction);

    Vector3 targetDirection = TargetDirection();
    float targetAngle = ((Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg) - StartAngle + 360) % 360;

    if ((PreviousAngle <= targetAngle) && (targetAngle <= angle)) {
      bool isCounterAttack = DamageTarget(1, true);
      if (isCounterAttack)
        DamageBoss.Damage(150);
    }

    PreviousAngle = angle;
  }

  public override void ControlRelinquished() {
    LineRenderer.enabled = false;
    DangerZoneImage.enabled = false;
  }

  public void LaserFinished() {
    RelinquishControlTo(WhenFinished);
  }
}
