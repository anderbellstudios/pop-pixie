using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

  public float FireRate;
  public int Capacity;
  public int Ammunition;
  public float Scatter = 0;
  public float BulletSpeed;
  public GameObject BulletPrefab;
  public Sprite Sprite;
  public Sprite InHandSprite;
  public AudioClip ShootSound;

  public float CooldownInterval() 
    => 1.0f / FireRate;

  public bool HasBullets() 
    => Ammunition > 0;

  public void ExpendBullet() {
    Ammunition = Mathf.Max(0, Ammunition - 1);
  }

  public bool Full() 
    => Ammunition == Capacity;

  public void Reload() {
    Ammunition = Capacity;
  }
}
