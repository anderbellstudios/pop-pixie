using System.Collections;
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
    w.FireRate = 0.5f;
    w.Capacity = 50;
    w.Ammunition = 50;
    w.BulletSpeed = 8.0f;
    w.BulletPrefab = (GameObject)Resources.Load(
      "Bullets/Pop Pellet", 
      typeof(GameObject)
    );

    return w;
  }
}
