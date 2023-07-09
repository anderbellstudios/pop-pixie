using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyAI : MonoBehaviour {

  public bool StartsInControl;
  public bool InControl;

  private LowPriorityBehaviour LowPriorityBehaviour;

  void Start() {
    LowPriorityBehaviour = new LowPriorityBehaviour();

    LocalStart();

    if ( StartsInControl )
      GainControl();
  }

  public virtual void LocalStart() {}

  public void GainControl() {
    InControl = true;
    ControlGained();
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    if ( InControl )
      WhileInControl();
  }

  public void RelinquishControl() {
    InControl = false;
    ControlRelinquished();
  }

  public virtual void ControlGained() {
  }

  public virtual void WhileInControl() {
  }

  public virtual void ControlRelinquished() {
  }

  void OnCollisionEnter2D( Collision2D col ) {
    if ( InControl )
      LocalOnCollisionEnter2D( col );
  }

  public virtual void LocalOnCollisionEnter2D( Collision2D col ) {
  }

  // Utility methods

  public void RelinquishControlTo( AEnemyAI ai ) {
    RelinquishControl();
    ai.GainControl();
  }

  public void ApplyMovement( Vector2 movement ) {
    GetComponent<MovementManager>().Movement += movement * Time.deltaTime;
  }

  public GameObject Target
    => PlayerGameObject.Current;

  public bool DamageTarget(float damage, bool canBeCounterAttacked = false) {
    return Target.GetComponent<HitPoints>().Damage(damage, canBeCounterAttacked);
  }

  public Vector2 TargetHeading() {
    return Target.transform.position - transform.position;
  }

  public float TargetDistance() {
    return TargetHeading().magnitude;
  }

  public Vector2 TargetDirection() {
    return TargetHeading().normalized;
  }

  private bool _LineOfMovement;

  public bool LineOfMovement() {
    LowPriorityBehaviour.EveryNFrames(10, () => {
      var hit = Physics2D.CircleCast( 
          transform.position, 
          WidthRequiredForMovement() / 2, 
          TargetDirection(),
          Mathf.Infinity,
          IgnoreEnemyLayerMask.Mask
          );

      _LineOfMovement = hit.collider.gameObject == Target;
    });

    return _LineOfMovement;
  }

  public float WidthRequiredForMovement() {
    return 1f;
  }

}
