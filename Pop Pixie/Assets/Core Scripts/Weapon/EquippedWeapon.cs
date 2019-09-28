using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour {

  public MonoBehaviour AmmunitionCircle;
  public ReloadIndicator ReloadIndicator;

  public Weapon CurrentWeapon;

  void Update () {
    var hc = (HUDCircle) AmmunitionCircle;
    hc.Progress = (float) CurrentWeapon.Ammunition / 
                  (float) CurrentWeapon.Capacity;

    ReloadIndicator.Visible = CurrentWeapon.Ammunition == 0;
  }

}
