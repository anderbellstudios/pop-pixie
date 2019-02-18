using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour {

  public float Duration;
  public MonoBehaviour ReloadBar;
  public MonoBehaviour SpeedManager;
  public bool InProgress;

  private DateTime StartedAt;

	// Use this for initialization
	void Start () {
    InProgress = false;

    // Reduce speed by half when reload is InProgress
    SpeedModifier modifier = s => InProgress ? 0.5f * s : s;

    var sm = (PlayerMovable) SpeedManager;
    sm.SpeedModifiers.Add(modifier);
	}
	
	// Update is called once per frame
	void Update () {
    var rb = (HUDBar) ReloadBar;

    if ( Input.GetButton("Reload") && CanReload() ) {
      if ( !InProgress ) {
        InProgress = true;
        StartedAt = DateTime.Now;
      }

      rb.Progress = Progress();

      if ( Progress() == 1.0f )
        CurrentWeapon().Reload();

    } else {
      InProgress = false;
    }

    rb.Visible = InProgress;
	}

  bool CanReload() {
    return !CurrentWeapon().Full();
  }

  float Progress() {
    float since = (float) DateTime.Now.Subtract( StartedAt ).TotalSeconds;
    return Mathf.Min( since / Duration, 1.0f );
  }

  Weapon CurrentWeapon() {
    return gameObject.GetComponent<EquippedWeapon>().CurrentWeapon;
  }
}
