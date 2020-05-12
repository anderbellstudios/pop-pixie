﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeapon : MonoBehaviour {

  public WeaponReload WeaponReload;
  public SpriteRenderer InHandSpriteRenderer;

  public PlayerWeapons PlayerWeapons;

  public List<PlayerWeapon> AvailableWeapons()
    => PlayerWeapons.AvailableWeapons();

  public int CurrentWeaponIndex;

  public PlayerWeapon CurrentWeapon
    => AvailableWeapons().Count > CurrentWeaponIndex
        ? AvailableWeapons()[CurrentWeaponIndex]
        : AvailableWeapons()[0];

  private WeaponInfoController WeaponInfoController;
  private HUDBar AmmunitionBar;
  private ReloadIndicator ReloadIndicator;

  void Start() {
    CurrentWeaponIndex = EquippedWeaponData.CurrentWeapon;

    WeaponInfoController = GameObject.Find("Weapon Info").GetComponent<WeaponInfoController>();

    AmmunitionBar = WeaponInfoController.AmmunitionBar;
    ReloadIndicator = WeaponInfoController.ReloadIndicator;

    UpdateWeaponSprites();
  }

  void Update () {
    PollChangeWeapon();
    UpdateWeaponInfo();
  }

  void PollChangeWeapon() {
    if ( StateManager.Isnt( State.Playing ) ) return;

    if ( WrappedInput.GetButtonDown("Next Weapon")     ) ChangeWeaponIndex(1);
    if ( WrappedInput.GetButtonDown("Previous Weapon") ) ChangeWeaponIndex(-1);
  }

  void UpdateWeaponInfo() {
    AmmunitionBar.Progress = (float) CurrentWeapon.Ammunition / 
                             (float) CurrentWeapon.Capacity;

    ReloadIndicator.Visible = CurrentWeapon.Ammunition == 0;
  }

  void ChangeWeaponIndex( int delta ) {
    CurrentWeaponIndex = RelativeWeaponIndex(delta);
    UpdateWeaponSprites();
    EquippedWeaponData.CurrentWeapon = CurrentWeaponIndex;
    WeaponReload.Interrupt();
  }

  void UpdateWeaponSprites() {
    InHandSpriteRenderer.sprite = CurrentWeapon.InHandSprite;

    Sprite prev = null, active = null, next = null;

    active = RelativeWeaponSprite(0);

    if ( AvailableWeapons().Count >= 2 )
      next = RelativeWeaponSprite(1);

    if ( AvailableWeapons().Count >= 3 )
      prev = RelativeWeaponSprite(-1);

    WeaponInfoController.SetWeaponSprites( prev, active, next );
  }

  int RelativeWeaponIndex( int delta ) {
    // positiveMod :: [-1,n] -> n -> [0,n)
    int positiveMod( int a, int n ) => (a + n) % n;

    return positiveMod(
      CurrentWeaponIndex + delta,
      AvailableWeapons().Count
    );
  }

  Sprite RelativeWeaponSprite( int delta ) {
    return AvailableWeapons()[
      RelativeWeaponIndex(delta)
    ].Sprite;
  }

}
