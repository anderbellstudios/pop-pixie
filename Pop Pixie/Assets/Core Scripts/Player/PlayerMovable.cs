using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float SpeedModifier(float s);

public class PlayerMovable : MonoBehaviour, IDirectionManager {

  public MovementManager MovementManager;
  public float Speed;
  public List<SpeedModifier> SpeedModifiers;
  public Vector3 Direction { get; set; }

  void Awake () {
    SpeedModifiers = new List<SpeedModifier>();
  }

  void FixedUpdate() {
    Direction = new Vector2(
      Input.GetAxis("Horizontal"),
      Input.GetAxis("Vertical")
    ).normalized;

    MovementManager.Movement += ModifiedSpeed() * (Vector2) Direction;
  }

  float ModifiedSpeed() {
    float speed = Speed;

    foreach (var modifier in SpeedModifiers) {
      speed = modifier(speed);
    }

    return speed;
  }
}
