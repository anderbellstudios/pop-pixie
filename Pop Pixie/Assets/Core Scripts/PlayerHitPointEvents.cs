using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitPointEvents : MonoBehaviour, IHitPointEvents {
  public void Decreased (HitPoints hp) {
    Debug.Log("HitPoints changed to");
    Debug.Log(hp.Current);
  }

  public void BecameZero (HitPoints hp) {
    Debug.Log("You died!");
  }
}
