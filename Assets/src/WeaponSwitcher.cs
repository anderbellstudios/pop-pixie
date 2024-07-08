using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeaponSwitcherUI : MonoBehaviour {
  public abstract void OnOpen(List<PlayerWeapon> availableWeapons, int equippedWeaponIndex);
  public abstract int OnClose();
}

public class WeaponSwitcher : MonoBehaviour {
  public bool SingletonInstance = true;
  public static WeaponSwitcher Current;

  public GameObject Canvas;
  public AWeaponSwitcherUI WeaponSwitcherUI;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  public void Show() {
    StateManager.AddState(State.Paused);
    Canvas.SetActive(true);

    WeaponSwitcherUI.OnOpen(
      availableWeapons: PlayerWeapons.Current.AvailableWeapons(),
      equippedWeaponIndex: EquippedWeapon.Current.CurrentWeaponIndex
    );
  }

  public void Hide() {
    int weaponIndex = WeaponSwitcherUI.OnClose();
    EquippedWeapon.Current.SetWeaponByIndex(weaponIndex);

    Canvas.SetActive(false);
    StateManager.RemoveState(State.Paused);
  }
}
