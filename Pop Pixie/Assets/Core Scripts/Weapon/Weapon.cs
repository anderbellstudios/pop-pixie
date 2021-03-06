﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon {

  public float FireRate;
  public int Capacity;
  public int Ammunition;
  public float BulletSpeed;
  public GameObject BulletPrefab;

  public static Weapon PopPistol(){
    var w = new Weapon();
    w.FireRate = 2.0f;
    w.Capacity = 12;
    w.Ammunition = 12;
    w.BulletSpeed = 8.0f;
    w.BulletPrefab = (GameObject)Resources.Load(
      "Bullets/Pop Pellet", 
      typeof(GameObject)
    );

    return w;
  }

  public bool HasBullets() {
    return Ammunition > 0;
  }

  public void ExpendBullet() {
    Ammunition = Mathf.Max(0, Ammunition - 1);
  }

  public bool Full() {
    return Ammunition == Capacity;
  }

  public void Reload() {
    Ammunition = Capacity;
  }
}
