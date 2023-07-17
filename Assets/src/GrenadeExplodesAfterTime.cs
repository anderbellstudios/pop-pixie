using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplodesAfterTime : MonoBehaviour {

  public BulletData BulletData;
  public Rigidbody2D Rigidbody;
  public GrenadeWaitingBeforeThrow GrenadeWaitingBeforeThrow;
  public float RadiusIndicatorTime;
  public float ExplodeTime;
  public float Radius;
  public float DamageExplodingInHand;
  public bool DamagesPlayer = true, DamagesEnemies = true;
  public float VelocityCoefficient;
  public AnimationCurve DamageCurve;
  public Transform RadiusIndicator;
  public GameObject Explosion;
  public GameObject ExplosionSound;

  private IntervalTimer RadiusIndicatorTimer;
  private IntervalTimer ExplodeTimer;

  void Awake() {
    RadiusIndicatorTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = RadiusIndicatorTime
    };

    ExplodeTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = ExplodeTime
    };

    ExplodeTimer.Reset();
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    if (!RadiusIndicatorTimer.Started && !WaitingToThrow() && ExplodeTimer.Progress() >= 0.50) {
      RadiusIndicatorTimer.Reset();
    }

    if (RadiusIndicatorTimer.Started) {
      SetRadiusIndicatorRadius(Radius * RadiusIndicatorTimer.Progress());
    }

    ExplodeTimer.IfElapsed(() => {
      bool isCounterAttack = DamageHitPointsInRadius.Invoke(
        damage: WaitingToThrow() ? DamageExplodingInHand : BulletData.Damage,
        origin: transform.position,
        radius: Radius,
        canBeCounterAttacked: true,
        damageCurve: DamageCurve,
        shouldDamage: (go) => {
          if (!DamagesPlayer && go.tag == "Player") return false;
          if (!DamagesEnemies && go.tag == "Enemy") return false;
          return true;
        }
      );

      if (isCounterAttack && BulletData.Originator && BulletData.Originator != PlayerGameObject.Current) {
        Vector3 toOriginator = (BulletData.Originator.transform.position - transform.position);
        Rigidbody.velocity = toOriginator * VelocityCoefficient;
        ExplodeTimer.Reset();
      } else {
        GameObject ExplosionGameObject = Instantiate(Explosion, transform.position, Quaternion.identity);
        ExplosionGameObject.transform.localScale = new Vector3(2 * Radius, 2 * Radius, 2 * Radius);

        if (ExplosionSound != null)
          Instantiate(ExplosionSound, transform.position, Quaternion.identity);

        Destroy(gameObject);
      }
    });
  }

  bool WaitingToThrow() => (GrenadeWaitingBeforeThrow != null) && GrenadeWaitingBeforeThrow.Waiting;

  void SetRadiusIndicatorRadius( float radius ) {
    RadiusIndicator.localScale = new Vector3( 2 * radius, 2 * radius, 2 * radius );
  }

}
