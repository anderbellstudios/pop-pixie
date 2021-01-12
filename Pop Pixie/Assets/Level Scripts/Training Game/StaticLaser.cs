using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLaser : MonoBehaviour {
  public void CollidedWithPlayer() {
    GameObject.FindGameObjectWithTag("Player").GetComponent<HitPoints>().Damage(1);
  }
}
