using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour {

  public SoundController SoundController;
  public HUDBar ReloadBar;
  public MovementManager MovementManager;
  private IntervalTimer ReloadTimer;
  private const float movementSpeedReduction = 0.5f;

	// Use this for initialization
	void Start () {
    InitializeReloadTimer();
	}
	
  private void InitializeReloadTimer() {
    ReloadTimer = new IntervalTimer(){TimeClass = "PlayingTime"};
    MovementManager.SpeedModifiers.Add(
      speed => InProgress() ? movementSpeedReduction * speed : speed
    );
  }

	void Update () {
    if (!StateManager.Playing)
      return;
    HandleReloading();
    
    if ( WrappedInput.GetButtonDown("Reload") && !InProgress() && CanReload() )
      BeginReload();

    UpdateReloadProgress();
    ReloadBar.Visible = InProgress();
	}

  private void UpdateReloadProgress() {
    if (!InProgress())
      return;

    if (CanReload())
      ReloadBar.Progress = ReloadTimer.Progress();
    else
      StopReloading();
  }

  private void HandleReloading() {
    ReloadTimer.IfElapsed( CurrentWeapon().Reload );

    if (UserTriggeredReload() && CanReload())
      BeginReload();

    UpdateReloadProgress();
  }
  private bool UserTriggeredReload() {
    return WrappedInput.GetButtonDown("Reload") && !InProgress();
  }

  private void StopReloading() {
    CurrentWeapon().isReloading = false;
    ReloadTimer.Stop();
    SoundController.Stop();
  }
  public void Interrupt() {
    CurrentWeapon().isReloading = false;
    ReloadTimer.Stop();
    SoundController.Stop();
  }

  void BeginReload() {
    Weapon weapon = CurrentWeapon().Weapon;
    CurrentWeapon().isReloading = true;
    ReloadTimer.Interval = weapon.ReloadDuration;
    ReloadTimer.Reset();
    SoundController.Play(weapon.ReloadSound);
  }

  bool InProgress() {
    return ReloadTimer.Started && !ReloadTimer.Elapsed();
  }

  bool CanReload() {
    return !CurrentWeapon().Full();
  }

  PlayerWeapon CurrentWeapon() {
    return gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
  }
}
