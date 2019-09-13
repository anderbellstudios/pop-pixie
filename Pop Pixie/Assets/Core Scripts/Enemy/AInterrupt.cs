using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AInterrupt : MonoBehaviour {
  
  void Start() {
    LocalStart();
  }

  public virtual void LocalStart() {}

  void Update() {
    AEnemyAI ai = GetComponents<AEnemyAI>().Where( x => x.InControl ).First();

    if ( !OnlyAIsMatching().IsInstanceOfType( ai ) )
      return;

    if ( ShouldInterrupt( ai ) )
      ai.RelinquishControlTo( InterruptAI() );
  }

  public virtual Type OnlyAIsMatching() {
    return typeof( AEnemyAI );
  }

  public abstract bool ShouldInterrupt( AEnemyAI ai );
  public abstract AEnemyAI InterruptAI();

}
