using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MentoeHologramSweepingAttackAI : AEnemyAI {
  public float SafeAngle;
  public float DangerZoneExpandDuration, BeforeLaserDuration, LaserDuration, LaserBeamLength;
  public AnimationCurve DangerZoneExpandCurve;
  public Transform DangerZoneTransform;
  public Image DangerZoneImage;
  public LineRenderer LineRenderer;
  public SoundHopper SoundHopper;

  public AEnemyAI WhenFinished;

  private IntervalTimer DangerZoneExpandTimer, BeforeLaserTimer, LaserTimer;

  private float StartAngle;
  private float PreviousAngle;

  public override void ControlGained() {
    DangerZoneExpandTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = DangerZoneExpandDuration
    };

    BeforeLaserTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = BeforeLaserDuration
    };

    DangerZoneExpandTimer.Reset();
    BeforeLaserTimer.Reset();

    LaserTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = LaserDuration
    };

    StartAngle = Random.Range(0, 360);
    DangerZoneImage.enabled = true;
    DangerZoneImage.fillAmount = 1 - (SafeAngle / 360);
    DangerZoneTransform.localRotation = Quaternion.Euler(0, 0, StartAngle - SafeAngle);

    PreviousAngle = 0;
  }

  public override void WhileInControl() {
    DangerZoneTransform.localScale = DangerZoneExpandCurve.Evaluate(DangerZoneExpandTimer.Progress()) * Vector3.one;

    BeforeLaserTimer.IfElapsed(() => {
      LaserTimer.Reset();
      BeforeLaserTimer.Stop();
      LineRenderer.enabled = true;
      SoundHopper.Hop();
    });

    if (LaserTimer.Elapsed()) {
      RelinquishControlTo(WhenFinished);
    } else if (LaserTimer.Started) {
      float angle = Mathf.Lerp(0, 360 - SafeAngle, LaserTimer.Progress());
      Vector3 direction = Quaternion.Euler(0, 0, StartAngle + angle) * Vector3.right;

      LineRenderer.SetPosition(0, transform.position);
      LineRenderer.SetPosition(1, transform.position + LaserBeamLength * direction);

      Vector3 targetDirection = TargetDirection();
      float targetAngle = ((Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg) - StartAngle + 360) % 360;

      if ((PreviousAngle <= targetAngle) && (targetAngle <= angle)) {
        DamageTarget(1);
      }

      PreviousAngle = angle;
    }
  }

  public override void ControlRelinquished() {
    LineRenderer.enabled = false;
    DangerZoneImage.enabled = false;
  }

  public void LaserFinished() {
    RelinquishControlTo(WhenFinished);
  }
}
