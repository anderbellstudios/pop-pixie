using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeapon : MonoBehaviour {

  public WeaponReload WeaponReload;
  public SpriteRenderer InHandSpriteRenderer;

  public List<Weapon> AllWeapons;
  public List<int> UnlockedWeaponIds;

  public int CurrentWeaponIdIndex;

  public Weapon CurrentWeapon
    => LookupWeapon( MapUnlockedWeaponIndexToId(CurrentWeaponIdIndex) );

  private WeaponInfoController WeaponInfoController;
  private HUDBar AmmunitionBar;
  private ReloadIndicator ReloadIndicator;

  void Start() {
    CurrentWeaponIdIndex = EquippedWeaponData.CurrentWeapon;

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
    CurrentWeaponIdIndex = RelativeWeaponIndex(delta);
    UpdateWeaponSprites();
    EquippedWeaponData.CurrentWeapon = CurrentWeaponIdIndex;
    WeaponReload.Interrupt();
  }

  void UpdateWeaponSprites() {
    InHandSpriteRenderer.sprite = CurrentWeapon.InHandSprite;

    Sprite prev = null, active = null, next = null;

    active = RelativeWeaponSprite(0);

    if ( UnlockedWeaponIds.Count >= 2 )
      next = RelativeWeaponSprite(1);

    if ( UnlockedWeaponIds.Count >= 3 )
      prev = RelativeWeaponSprite(-1);

    WeaponInfoController.SetWeaponSprites( prev, active, next );
  }

  int RelativeWeaponIndex( int delta ) {
    // positiveMod :: [-1,n] -> n -> [0,n)
    int positiveMod( int a, int n ) => (a + n) % n;

    return positiveMod(
      CurrentWeaponIdIndex + delta,
      UnlockedWeaponIds.Count
    );
  }

  Sprite RelativeWeaponSprite( int delta ) {
    return LookupWeapon(
      MapUnlockedWeaponIndexToId(
        RelativeWeaponIndex(delta)
      )
    ).Sprite;
  }

  private int MapUnlockedWeaponIndexToId( int index )
    => UnlockedWeaponIds[ index ];

  private Weapon LookupWeapon( int id )
    => AllWeapons[ id ];

}
