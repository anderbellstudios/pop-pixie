using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour {

  public SoundController SoundController;
  public HUDBar ReloadBar;
  public MovementManager MovementManager;

  IntervalTimer ReloadTimer;

  // Use this for initialization
  void Start() {
    ReloadTimer = new IntervalTimer() {
      TimeClass = "PlayingTime"
    };

    // Reduce speed by half when reload is InProgress
    MovementManager.SpeedModifiers.Add(
      s => InProgress() ? 0.5f * s : s
    );

  }

  // Update is called once per frame
  void Update() {
    if (!StateManager.Playing)
      return;

    ReloadTimer.IfElapsed(CurrentWeapon().Reload);

    if (WrappedInput.GetButtonDown("Reload") && !InProgress() && CanReload())
      BeginReload();

    if (InProgress()) {
      if (CanReload()) {
        ReloadBar.Progress = ReloadTimer.Progress();
      } else {
        ReloadTimer.Stop();
        SoundController.Stop();
      }
    }

    ReloadBar.Visible = InProgress();
  }

  public void Interrupt() {
    ReloadTimer.Stop();
    SoundController.Stop();
  }

  void BeginReload() {
    Weapon weapon = CurrentWeapon().Weapon;
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
