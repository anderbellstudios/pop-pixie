using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour {
  public PlaySound PlaySound;
  public MovementManager MovementManager;

  private Stopwatch ReloadStopwatch = null;

  void Start() {
    // Reduce speed by half when reload is InProgress
    MovementManager.SpeedModifiers.Add(
      s => InProgress() ? 0.5f * s : s
    );
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    PlayerWeapon weapon = CurrentWeapon();
    float progress = ReloadStopwatch?.Progress(weapon.ReloadDuration) ?? 0f;

    if (progress >= 1f) {
      weapon.Reload();
      StopReloading();
    }

    if (WrappedInput.GetButtonDown("Reload") && !InProgress() && CanReload()) {
      BeginReload();
    }

    if (InProgress()) {
      if (CanReload()) {
        HUDBar.Reload?.SetProgress(progress);
      } else {
        InterruptReloading();
      }
    }

    HUDBar.Reload?.SetVisible(InProgress());
  }

  private void BeginReload() {
    PlayerWeapon weapon = CurrentWeapon();
    ReloadStopwatch = new Stopwatch.PlayingTime();
    PlaySound.Play(weapon.ReloadSoundKey);
  }

  public void InterruptReloading() {
    StopReloading();
    PlaySound.Stop();
  }

  private void StopReloading() {
    ReloadStopwatch = null;
  }

  private bool InProgress() => ReloadStopwatch != null;
  private bool CanReload() => !CurrentWeapon().Full();

  private PlayerWeapon CurrentWeapon()
    => gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
}
