using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramHoloringAttackAI : ARepeatedAttackAI {
  public GameObject HoloringPrefab;

  public override void PerformAttack() {
    Instantiate(HoloringPrefab, transform);
  }
}
