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
  public float TapToChangeDuration;

  private WeaponInfoController WeaponInfoController;
  private bool ListeningForTap = false;
  private AsyncTimer.EnqueuedEvent TapTimer;
  private bool WeaponSwitcherOpen = false;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  void Start() {
    WeaponInfoController = GameObject.Find("Weapon Info").GetComponent<WeaponInfoController>();

    SetWeaponByIndex(index: CurrentWeaponIndex, setLast: false);

    InGamePrompt.Current.RegisterSource(200, () =>
      CurrentWeapon.Ammunition == 0
        ? "Press [Reload] to reload your weapon"
        : null
    );

    StateManager.AddListener(() => {
      if (!StateManager.Playing) {
        AsyncTimer.BaseTime.ClearTimeout(TapTimer);
        ListeningForTap = false;
      }
    });
  }

  void Update() {
    WeaponInfoController.SetAmmunition(CurrentWeapon.Ammunition, CurrentWeapon.Capacity);

    if (StateManager.Playing && WrappedInput.GetButtonDown("Change Weapon")) {
      WeaponReload.Interrupt();

      ListeningForTap = true;

      TapTimer = AsyncTimer.BaseTime.SetTimeout(() => {
        // Not a tap, so open the weapon switcher
        WeaponSwitcher.Current.Show();
        WeaponSwitcherOpen = true;

        ListeningForTap = false;
      }, TapToChangeDuration);
    }

    if (WrappedInput.GetButtonUp("Change Weapon")) {
      if (WeaponSwitcherOpen) {
        WeaponSwitcher.Current.Hide();
        WeaponSwitcherOpen = false;
      }

      if (ListeningForTap) {
        AsyncTimer.BaseTime.ClearTimeout(TapTimer);
        ListeningForTap = false;
        HandleTap();
      }
    }
  }

  void HandleTap() {
    /**
     * If LastWeaponIndex is -1, do nothing. This serves to teach the player
     * that the full weapon switcher exists. Otherwise, a player might toggle
     * between their first two weapons without realising that other weapons are
     * available.
     */
    if (LastWeaponIndex != -1) {
      SetWeaponByIndex(LastWeaponIndex);
    }
  }

  public void SetWeaponByIndex(int index, bool setLast = true) {
    CurrentWeapon = AvailableWeapons.Count > index
      ? AvailableWeapons[index]
      : AvailableWeapons[0];

    if (setLast && CurrentWeaponIndex != index) {
      LastWeaponIndex = CurrentWeaponIndex;
    }

    CurrentWeaponIndex = index;

    InHandSpriteRenderer.sprite = CurrentWeapon.InHandSprite;
    WeaponInfoController.SetWeaponImage(CurrentWeapon.Sprite);
    WeaponInfoController.SetWeaponName(CurrentWeapon.Name);
  }

  public int CurrentWeaponIndex {
    get {
      return EquippedWeaponData.CurrentWeapon;
    }

    set {
      EquippedWeaponData.CurrentWeapon = value;
    }
  }

  public int LastWeaponIndex {
    get {
      return EquippedWeaponData.LastWeapon;
    }

    set {
      EquippedWeaponData.LastWeapon = value;
    }
  }

  public List<PlayerWeapon> AvailableWeapons => PlayerWeapons.AvailableWeapons();
}
