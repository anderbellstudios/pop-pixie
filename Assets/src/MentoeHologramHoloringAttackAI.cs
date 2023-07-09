using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramHoloringAttackAI : ARepeatedAttackAI {
  public GameObject HoloringPrefab;
  public DamageMultiHitPointEntity DamageBoss;

  void Start() {
    InGamePrompt.Current.RegisterSource(98, () =>
      InControl
      ? "Press <size=150%>[Roll]</size> while moving to <color=#ffff00>roll</color>"
      : null
    );
  }

  public override void PerformAttack() {
    GameObject holoringGameObject = Instantiate(HoloringPrefab, transform);
    Holoring holoring = holoringGameObject.GetComponent<Holoring>();
    holoring.DamageBoss = (damage) => DamageBoss.Damage(damage);
  }
}
