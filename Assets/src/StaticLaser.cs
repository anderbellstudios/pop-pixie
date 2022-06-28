using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLaser : MonoBehaviour {
  public void CollidedWithPlayer() {
    PlayerGameObject.Current.GetComponent<HitPoints>().Damage(1);
  }
}
