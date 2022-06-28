using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovable : MonoBehaviour, IDirectionManager {

  public MovementManager MovementManager;
  public float Speed;
  public Roll Roll;
  public Vector3 Direction { get; set; }

  void Update() {
    if (!StateManager.Playing)
      return;

    Direction = new Vector2(
      WrappedInput.GetAxis("Horizontal"),
      WrappedInput.GetAxis("Vertical")
    ).normalized;

    if ( !Roll.Rolling )
      MovementManager.Movement += Speed * (Vector2) Direction * Time.deltaTime;
  }

}
