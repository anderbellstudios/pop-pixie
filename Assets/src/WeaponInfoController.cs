using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoController : MonoBehaviour {
  public HUDBar AmmunitionBar;
  public ReloadIndicator ReloadIndicator;

  public Image PreviousWeapon, ActiveWeapon, NextWeapon;

  public void SetWeaponSprites( Sprite prev, Sprite active, Sprite next ) {
    SetNullableSprite( PreviousWeapon, prev );
    SetNullableSprite( ActiveWeapon, active );
    SetNullableSprite( NextWeapon, next );
  }

  private void SetNullableSprite( Image image, Sprite sprite ) {
    if ( sprite == null ) {
      image.enabled = false;
    } else {
      image.sprite = sprite;
      image.enabled = true;
    }
  }
}
