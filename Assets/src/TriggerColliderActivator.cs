using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderActivator : AActivator {

  public List<GameObject> CollidingObjects;

  void OnTriggerEnter2D( Collider2D col ) {
    CollidingObjects.Add( col.gameObject );
  }

  void OnTriggerExit2D( Collider2D col ) {
    CollidingObjects.Remove( col.gameObject );
  }

  public override bool IsActivated( AEnemyAI ai ) {
    return CollidingObjects.Contains( ai.Target );
  }

}
