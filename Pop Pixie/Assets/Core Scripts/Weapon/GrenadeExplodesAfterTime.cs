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
  public AnimationCurve DamageCurve;
  public Transform RadiusIndicator;
  public GameObject Explosion;

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
    if (StateManager.Isnt( State.Playing ))
      return;

    if (!RadiusIndicatorTimer.Started && !GrenadeWaitingBeforeThrow.Waiting && ExplodeTimer.Progress() >= 0.50) {
      RadiusIndicatorTimer.Reset();
    }

    if (RadiusIndicatorTimer.Started) {
      SetRadiusIndicatorRadius(Radius * RadiusIndicatorTimer.Progress());
    }

    ExplodeTimer.IfElapsed(() => {
      GameObject ExplosionGameObject = Instantiate(Explosion, transform.position, Quaternion.identity);
      ExplosionGameObject.transform.localScale = new Vector3(2 * Radius, 2 * Radius, 2 * Radius);
      ApplyDamageInRadius();
      Destroy(gameObject);
    });
  }

  void ApplyDamageInRadius() {
    foreach (GameObject go in FindObjectsOfType<GameObject>()) {
      HitPoints hp = go.GetComponent<HitPoints>();
      float distance = (go.transform.position - transform.position).magnitude;

      if (hp != null && distance <= Radius) {
        hp.Damage(BulletData.Damage * DamageCurve.Evaluate(distance / Radius));
      }
    };
  }

  void SetRadiusIndicatorRadius( float radius ) {
    RadiusIndicator.localScale = new Vector3( 2 * radius, 2 * radius, 2 * radius );
  }

}
