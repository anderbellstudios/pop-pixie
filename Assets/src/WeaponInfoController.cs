using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoController : MonoBehaviour {
  public HUDBar AmmunitionBar;
  public ReloadIndicator ReloadIndicator;
  public Image WeaponImage;
  public TMP_Text WeaponName;

  public void SetWeaponImage(Sprite image) {
    if (image == null) {
      WeaponImage.enabled = false;
    } else {
      WeaponImage.sprite = image;
      WeaponImage.enabled = true;
    }
  }

  public void SetWeaponName(string name) {
    WeaponName.text = name;
  }

  public void SetAmmunition(int current, int max) {
    AmmunitionBar.Progress = (float) current / (float) max;
    ReloadIndicator.Visible = current == 0;
  }
}
