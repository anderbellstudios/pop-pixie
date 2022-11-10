using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeapon : MonoBehaviour {
  public bool SingletonInstance = true;
  public static EquippedWeapon Current;

  public WeaponReload WeaponReload;
  public SpriteRenderer InHandSpriteRenderer;
  public PlayerWeapons PlayerWeapons;
  public PlayerWeapon CurrentWeapon;
  private WeaponInfoController WeaponInfoController;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  void Start() {
    WeaponInfoController = GameObject.Find("Weapon Info").GetComponent<WeaponInfoController>();

    SetWeaponByIndex(EquippedWeaponData.CurrentWeapon);

    InGamePrompt.Current.RegisterSource(200, () =>
      CurrentWeapon.Ammunition == 0
        ? "Press [Reload] to reload your weapon"
        : null
    );
  }

  void Update() {
    WeaponInfoController.SetAmmunition(CurrentWeapon.Ammunition, CurrentWeapon.Capacity);

    if (StateManager.Playing && WrappedInput.GetButtonDown("Change Weapon")) {
      SetWeaponByIndex((EquippedWeaponData.CurrentWeapon + 1) % AvailableWeapons.Count);
      WeaponReload.Interrupt();
    }
  }

  void SetWeaponByIndex(int index) {
    CurrentWeapon = AvailableWeapons.Count > index
      ? AvailableWeapons[index]
      : AvailableWeapons[0];

    EquippedWeaponData.CurrentWeapon = index;

    InHandSpriteRenderer.sprite = CurrentWeapon.InHandSprite;
    WeaponInfoController.SetWeaponImage(CurrentWeapon.Sprite);
    WeaponInfoController.SetWeaponName(CurrentWeapon.Name);
  }

  public List<PlayerWeapon> AvailableWeapons => PlayerWeapons.AvailableWeapons();
}
