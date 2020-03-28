using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovable : MonoBehaviour, IDirectionManager {

  public MovementManager MovementManager;
  public Animator Animator;
  public float Speed;
  public Roll Roll;
  public Vector3 Direction { get; set; }

  void FixedUpdate() {
    Direction = new Vector2(
      WrappedInput.GetAxis("Horizontal"),
      WrappedInput.GetAxis("Vertical")
    );

    int facing = Direction.x >= 0 ? 1 : -1;
    Animator.SetInteger("Direction", facing);

    if ( !Roll.Rolling )
      MovementManager.Movement += Speed * (Vector2) Direction;
  }

}
