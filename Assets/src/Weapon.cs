using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon", order = 1)]
[System.Serializable]
public class Weapon : ScriptableObject {

  public string Id;
  public string Name;
  public bool StartingWeapon = false;
  public float Damage;
  public float FireRate;
  public int Capacity;
  public float Scatter = 0;
  public float BulletSpeed;
  public float ReloadDuration = 2f;
  public GameObject BulletPrefab;
  public Sprite Sprite;
  public Sprite InHandSprite;
  public AudioClip ReloadSound, ShootSound;

  public float CooldownInterval() 
    => 1.0f / FireRate;
}
