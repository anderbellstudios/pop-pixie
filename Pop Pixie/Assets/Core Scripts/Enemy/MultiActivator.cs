using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiActivator : AActivator {

  public bool RequireAll;
  public List<AActivator> Activators;

  public override bool IsActivated( AEnemyAI ai ) {
    if ( RequireAll ) {
      return Activators.All( a => a.IsActivated(ai) );
    } else {
      return Activators.Any( a => a.IsActivated(ai) );
    }
  }

}
