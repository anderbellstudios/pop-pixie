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
    return new Weapon() {
      FireRate = 2.0f,
      Capacity = 12,
      Ammunition = 12,
      BulletSpeed = 60.0f,
      BulletPrefab = (GameObject)Resources.Load(
        "Bullets/Pop Pellet", 
        typeof(GameObject)
      )
    };
  }

  public static Weapon Turret(){
    return new Weapon() {
      FireRate = 5.0f,
      Capacity = 0,
      Ammunition = 0,
      BulletSpeed = 10.0f,
      BulletPrefab = (GameObject)Resources.Load(
        "Bullets/Turret Bullet", 
        typeof(GameObject)
      )
    };
  }

  public float CooldownInterval() {
    return 1.0f / FireRate;
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
