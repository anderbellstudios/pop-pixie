using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour {
  public static Crouch Current;

  public GameObject GameObject;
  public MovementManager MovementManager;
  public Animator Animator;
  public Roll Roll;
  public SpriteRenderer AimingArrow;
  public float SpeedMultiplier;

  public bool Crouching { get; private set; }

  private bool _InCrouchZone = false;
  public bool InCrouchZone => Crouching && _InCrouchZone;

  void Awake() {
    Current = this;
  }

  void Start() {
    MovementManager.SpeedModifiers.Add(
      s => Crouching ? SpeedMultiplier * s : s
    );
  }

  public void TrySetCrouching(bool crouching) {
    if (crouching == Crouching)
      return;

    if (crouching && CanCrouch())
      SetCrouching(true);

    if (!crouching && CanUncrouch())
      SetCrouching(false);
  }

  public void SetInCrouchZone(bool inCrouchZone) {
    _InCrouchZone = inCrouchZone;
    UpdateAimingArrowEnabled();
  }

  private void SetCrouching(bool crouching) {
    Crouching = crouching;
    Animator.SetBool("Crouching", crouching);
    GameObject.layer = LayerMask.NameToLayer(
      crouching ? "PlayerCrouching" : "Player"
    );
    UpdateAimingArrowEnabled();
  }

  private void UpdateAimingArrowEnabled() {
    AimingArrow.enabled = !InCrouchZone;
  }

  private bool CanCrouch() => !Roll.Rolling;
  private bool CanUncrouch() => !InCrouchZone;
}
