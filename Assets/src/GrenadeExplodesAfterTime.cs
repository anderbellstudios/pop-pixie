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
  public bool IsDestructive = true;
  public float VelocityCoefficient;
  public AnimationCurve DamageCurve;
  public Transform RadiusIndicator;
  public GameObject Explosion;
  public GameObject ExplosionSound;
  public GameObject Debris;

  private Stopwatch ExplodeStopwatch;
  private Stopwatch RadiusIndicatorStopwatch = null;

  void Awake() {
    ExplodeStopwatch = new Stopwatch.PlayingTime();
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    if (RadiusIndicatorStopwatch == null) {
      if (!WaitingToThrow() && ExplodeProgress() > 0.5f) {
        RadiusIndicatorStopwatch = new Stopwatch.PlayingTime();
      }
    } else {
      SetRadiusIndicatorRadius(Radius * RadiusIndicatorProgress());
    }

    if (ExplodeProgress() >= 1f) {
      Explode();
    }
  }

  private void Explode() {
    bool isCounterAttack = DamageHitPointsInRadius.Invoke(
      damage: WaitingToThrow() ? DamageExplodingInHand : BulletData.Damage,
      origin: transform.position,
      radius: Radius,
      canBeCounterAttacked: true,
      isDestructive: IsDestructive,
      damageCurve: DamageCurve,
      shouldDamage: (go) => {
        if (!DamagesPlayer && go.tag == "Player")
          return false;
        if (!DamagesEnemies && go.tag == "Enemy")
          return false;
        return true;
      }
    );

    // Send grenade back to originator on counterattack 
    if (isCounterAttack && BulletData.Originator && BulletData.Originator != PlayerGameObject.Current) {
      Vector3 toOriginator = (BulletData.Originator.transform.position - transform.position);
      Rigidbody.velocity = toOriginator * VelocityCoefficient;
      DamagesEnemies = true;
      ExplodeStopwatch.Reset();
      return;
    }

    GameObject ExplosionGameObject = Instantiate(Explosion, transform.position, Quaternion.identity);
    ExplosionGameObject.transform.localScale = new Vector3(2 * Radius, 2 * Radius, 2 * Radius);

    if (ExplosionSound != null)
      Instantiate(ExplosionSound, transform.position, Quaternion.identity);

    if (Debris != null)
      Instantiate(Debris, transform.position, Quaternion.identity);

    Destroy(gameObject);
  }

  private float ExplodeProgress() => ExplodeStopwatch.Progress(ExplodeTime);
  private float RadiusIndicatorProgress() => RadiusIndicatorStopwatch.Progress(RadiusIndicatorTime);
  private bool WaitingToThrow() => (GrenadeWaitingBeforeThrow != null) && GrenadeWaitingBeforeThrow.Waiting;

  private void SetRadiusIndicatorRadius(float radius) {
    RadiusIndicator.localScale = 2f * radius * Vector3.one;
  }
}
