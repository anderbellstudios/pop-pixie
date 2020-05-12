using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapons : MonoBehaviour {

  public List<Weapon> AllWeapons;

  private List<PlayerWeapon> _AvailableWeapons;
  public List<PlayerWeapon> AvailableWeapons() {
    if (_AvailableWeapons == null) {
      _AvailableWeapons = AllWeapons.Select(w => new PlayerWeapon(w)).ToList();
    }

    return _AvailableWeapons;
  }

}

public class PlayerWeapon {
  public Weapon Weapon;

  public int Ammunition {
    set {
      GameData.Current.Set("ammunition-" + Weapon.Id, value);
    }
    get {
      return GameData.Current.Fetch("ammunition-" + Weapon.Id, orSetEqualTo: Weapon.Capacity);
    }
  }

  public PlayerWeapon(Weapon weapon) {
    Weapon = weapon;
  }

  public string Id => Weapon.Id;
  public float FireRate => Weapon.FireRate;
  public int Capacity => Weapon.Capacity;
  public float Scatter => Weapon.Scatter;
  public float BulletSpeed => Weapon.BulletSpeed;
  public GameObject BulletPrefab => Weapon.BulletPrefab;
  public Sprite Sprite => Weapon.Sprite;
  public Sprite InHandSprite => Weapon.InHandSprite;
  public AudioClip ShootSound => Weapon.ShootSound;

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
