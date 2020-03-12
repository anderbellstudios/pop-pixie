using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

  public float FireRate;
  public int Capacity;
  public int Ammunition;
  public float BulletSpeed;
  public GameObject BulletPrefab;
  public Sprite Sprite;

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
