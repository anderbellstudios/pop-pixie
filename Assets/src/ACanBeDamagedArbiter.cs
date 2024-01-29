using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACanBeDamagedArbiter : MonoBehaviour {
  public virtual bool CanBeDamaged(HitPoints hp, float damage) {
    return true;
  }
}
