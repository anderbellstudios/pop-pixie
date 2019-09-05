using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyAI : MonoBehaviour {

  public bool StartsInControl;
  public bool InControl;

  void Start() {
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
    if ( StateManager.Isnt( State.Playing ) ) 
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
    GetComponent<MovementManager>().Movement += movement;
  }

  public GameObject Target {
    get { return GameObject.FindGameObjectWithTag("Player"); }
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

  public bool LineOfMovement() {
    var hit = Physics2D.CircleCast( 
      transform.position, 
      WidthRequiredForMovement() / 2, 
      TargetDirection(),
      Mathf.Infinity,
      ~( ( 1 << 8 ) | ( 1 << 9 ) ) // <-- neither 8 nor 9
    );

    return hit.collider.gameObject == Target;
  }

  public float WidthRequiredForMovement() {
    return 1f;
  }

}
