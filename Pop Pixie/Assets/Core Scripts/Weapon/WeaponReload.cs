using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour {

  public float Duration;
  public MonoBehaviour ReloadBar;
  public MovementManager MovementManager;

  IntervalTimer ReloadTimer;

	// Use this for initialization
	void Start () {
    ReloadTimer = new IntervalTimer() {
      Interval = Duration
    };

    // Reduce speed by half when reload is InProgress
    MovementManager.SpeedModifiers.Add(
      s => InProgress() ? 0.5f * s : s
    );

	}
	
	// Update is called once per frame
	void Update () {
    var rb = (HUDBar) ReloadBar;

    ReloadTimer.IfElapsed( CurrentWeapon().Reload );

    if ( WrappedInput.GetButton("Reload") && !InProgress() && CanReload() )
      ReloadTimer.Reset();

    if ( InProgress() && CanReload() ) {
      rb.Progress = Progress();
    } else {
      ReloadTimer.Stop();
    }

    rb.Visible = InProgress();
	}

  bool InProgress() {
    return ReloadTimer.Started && !ReloadTimer.Elapsed();
  }

  bool CanReload() {
    return !CurrentWeapon().Full() && StateManager.Is( State.Playing );
  }

  float Progress() {
    return Mathf.Clamp(
      ReloadTimer.TimeSinceElapsed() / Duration,
      0f,
      1f
    );
  }

  Weapon CurrentWeapon() {
    return gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
  }
}
