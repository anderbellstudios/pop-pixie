using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour {
  public MovementManager MovementManager;
  public Animator Animator;
  public float SpeedMultiplier;

  public bool Crouching { get; private set; }

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

  private void SetCrouching(bool crouching) {
    Crouching = crouching;
    Animator.SetBool("Crouching", crouching);
  }

  private bool CanCrouch() => true;
  private bool CanUncrouch() => true;
}
