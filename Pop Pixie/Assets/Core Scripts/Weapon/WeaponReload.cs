using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour {

  public float Duration;
  public AudioClip ReloadSound;
  public SoundController SoundController;
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

    if ( WrappedInput.GetButtonDown("Reload") && !InProgress() && CanReload() )
      BeginReload();

    if ( InProgress() ) {
      if ( CanReload() ) {
        rb.Progress = Progress();
      } else {
        ReloadTimer.Stop();
        SoundController.Stop();
      }
    }

    rb.Visible = InProgress();
	}

  void BeginReload() {
    ReloadTimer.Reset();
    SoundController.Play( ReloadSound );
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
