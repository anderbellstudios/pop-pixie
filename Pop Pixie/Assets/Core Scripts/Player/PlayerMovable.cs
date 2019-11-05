using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovable : MonoBehaviour, IDirectionManager {

  public MovementManager MovementManager;
  public float Speed;
  public Roll Roll;
  public Vector3 Direction { get; set; }

  void FixedUpdate() {
    Direction = new Vector2(
      Input.GetAxis("Horizontal"),
      Input.GetAxis("Vertical")
    );

    if ( !Roll.Rolling )
      MovementManager.Movement += Speed * (Vector2) Direction;
  }

}
