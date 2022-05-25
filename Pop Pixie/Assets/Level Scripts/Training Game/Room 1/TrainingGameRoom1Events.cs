using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingGameRoom1Events : MonoBehaviour {
  public ScreenFade Fader;
  public List<GameObject> Targets;

  private bool TargetsDestroyed = false;

  void Start() {
    Fader.Fade("to black", 0.0f);

    SimulationResultData.StartedTime = DateTime.Now;
    SimulationResultData.NumberOfHitsTaken = 0;
    SimulationResultData.ObstacleCourseBestTime = null;

    // Drain all available weapons to force player to reload
    PlayerWeapons.Current.AvailableWeapons().ForEach(weapon => weapon.Ammunition = 0);

    EquippedWeapon equippedWeapon = EquippedWeapon.Current;

    InGamePrompt.Current.RegisterSource(101, () =>
      equippedWeapon.NeedToReload()
      ? "Press [Reload] to load your weapon"
      : null
    );

    InGamePrompt.Current.RegisterSource(100, () =>
      Targets.Any(t => EnemyUtils.IsDead(t))
      ? null
      : "Aim and press [Fire] to shoot the <color=#ffff00>Hologrems</color>"
    );
  }

  public void FadeIn() {
    Fader.Fade("from black", 1f);
  }
}
