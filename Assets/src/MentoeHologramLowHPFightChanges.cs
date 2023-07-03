using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeHologramLowHPFightChanges : MonoBehaviour {
  public HitPoints EnclosureHitPoints, BossHitPoints;
  public ARepeatedAttackAI HoloringAI, BombAI;
  public MentoeHologramSweepingAttackAI SweepingAI;
  public MentoeHologramBulletsAttackAI BulletsAI;
  public float BreakEnclosureSpeedMultiplier, TwoThirdsHPSpeedMultiplier, OneThirdHPSpeedMultiplier;

  private bool BelowTwoThirds, BelowOneThird = false;

  void Awake() {
    EnclosureHitPoints.OnBecomeZero.AddListener(HandleBreakEnclosure);
    BossHitPoints.OnDecrease.AddListener(HandleBossDecrease);
  }

  void HandleBreakEnclosure(HitPoints hitPoints) {
    MultiplyAttackSpeed(BreakEnclosureSpeedMultiplier);
  }

  void HandleBossDecrease(HitPoints hitPoints) {
    float fractionalHitPoints = hitPoints.Current / hitPoints.Maximum;

    if (!BelowTwoThirds && fractionalHitPoints < 0.67f) {
      BelowTwoThirds = true;
      MultiplyAttackSpeed(TwoThirdsHPSpeedMultiplier);
    }

    if (!BelowOneThird && fractionalHitPoints < 0.33f) {
      BelowOneThird = true;
      MultiplyAttackSpeed(OneThirdHPSpeedMultiplier);
    }
  }

  void MultiplyAttackSpeed(float multiplier) {
    HoloringAI.AttackInterval /= multiplier;
    BombAI.AttackInterval /= multiplier;
    SweepingAI.BeforeLaserDuration /= multiplier;
    BulletsAI.Rotations *= multiplier;
  }
}
