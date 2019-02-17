using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour {

  // Can't think of a good name for this property.
  // Both "EquippedWeapon" and "Weapon" are taken.
  // anEquippedWeapon.CurrentWeapon looks ugly,
  // but it's better than breaking naming conventions.
  public Weapon CurrentWeapon;

	// Use this for initialization
	void Start () {
    CurrentWeapon = Weapon.PopPistol();
	}

}
