using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingGamePlayerHitPointEvents : MonoBehaviour, IHitPointEvents {

  public void Updated (HitPoints hp) {}

  public void Decreased (HitPoints hp) {
    DamagedBlur.Current.Activate();
  }

  public void BecameZero (HitPoints hp) {}
}
