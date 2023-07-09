using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACounterAttackArbiter : MonoBehaviour {
  public virtual bool IsCounterAttack() {
    return false;
  }
}
