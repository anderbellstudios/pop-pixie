using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AActivator : MonoBehaviour {

  public virtual bool IsActivated( AEnemyAI ai ) {
    return IsActivated();
  }

  public virtual bool IsActivated() {
    throw new System.NotImplementedException("No implementation provided for IsActivated");
  }

}
