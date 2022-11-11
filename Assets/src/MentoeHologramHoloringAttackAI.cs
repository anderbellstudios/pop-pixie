using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramHoloringAttackAI : ARepeatedAttackAI {
  public GameObject HoloringPrefab;

  void Start() {
    InGamePrompt.Current.RegisterSource(98, () =>
      InControl
      ? "Press <size=150%>[Roll]</size> while moving to <color=#ffff00>roll</color>"
      : null
    );
  }

  public override void PerformAttack() {
    Instantiate(HoloringPrefab, transform);
  }
}
