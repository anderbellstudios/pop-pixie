using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingGameRoom1Events : MonoBehaviour {
  public List<GameObject> Targets;

  void Start() {
    ScreenFade.FadeOut(0f);

    SimulationResultData.StartedTime = PlayingTime.time;
    SimulationResultData.NumberOfHitsTaken = 0;
    SimulationResultData.ObstacleCourseBestTime = null;

    // Drain all available weapons to force player to reload
    PlayerWeapons.Current.AvailableWeapons().ForEach(weapon => weapon.Ammunition = 0);

    EquippedWeapon equippedWeapon = EquippedWeapon.Current;

    InGamePrompt.Current.RegisterSource(
      InGamePrompt.Priority.TutorialFire,
      () =>
        Targets.Any(t => EnemyUtils.IsDead(t))
          ? null
          : "Aim and press [Fire] to shoot the <color=#ffff00>Hologrems</color>"
    );
  }

  public void FadeIn() {
    ScreenFade.FadeIn(1f);
  }
}
