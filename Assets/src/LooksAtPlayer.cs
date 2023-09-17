using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooksAtPlayer : MonoBehaviour, IDirectionManager {

  public GameObject player;
  public float k, w_squared;
  public float Velocity;

  public Vector3 Direction { get; set; }

  void Start() {
    player = GameObject.Find("Pixie");
  }

  void Update() {
    Accelerate(w_squared * RelativeAngle());
    Accelerate(-k * Velocity);

    VelocityTick();

    Direction = transform.right;
  }

  void Accelerate(float amount) {
    Velocity += amount * Time.deltaTime;
  }

  void VelocityTick() {
    transform.Rotate(0, 0, Velocity * Time.deltaTime);
  }

  float RelativeAngle() {
    return Mathf.Deg2Rad * Vector3.SignedAngle(
      transform.right,
      TargetDirection(),
      Vector3.forward
    );
  }

  Vector3 TargetDirection() {
    return player.transform.position - transform.position;
  }
}
