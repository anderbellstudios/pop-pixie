using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameRoom1Events : MonoBehaviour {
  public List<GameObject> Targets;

  private bool TargetsDestroyed = false;

  void Start() {
    // Drain all available weapons to force player to reload
    PlayerWeapons.Current.AvailableWeapons().ForEach(weapon => weapon.Ammunition = 0);

    EquippedWeapon equippedWeapon = EquippedWeapon.Current;

    InGamePrompt.Current.RegisterSource(() =>
      equippedWeapon.NeedToReload()
      ? "Press <color=#00ffff>[Reload]</color> to load your weapon"
      : null
    );

    InGamePrompt.Current.RegisterSource(() =>
      Targets.Any(t => EnemyUtils.IsDead(t))
      ? null
      : "Aim and press <color=#00ffff>[Fire]</color> to shoot the <color=#ffff00>Hologrems</color>"
    );
  }

  void Update() {
    if (!TargetsDestroyed && Targets.All(t => EnemyUtils.IsDead(t))) {
      TargetsDestroyed = true;
      SceneManager.LoadScene("Training Room 2");
    }
  }
}
