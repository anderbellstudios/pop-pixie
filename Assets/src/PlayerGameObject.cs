using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameObject : MonoBehaviour {
  public static GameObject Current;

  public static Vector3 EstimatedVelocity => Vector3.ClampMagnitude(EstimatedDirection, Speed);

  private static float Speed;
  private static Vector3 LastPosition;
  private static Vector3 EstimatedDirection;

  void Awake() {
    Current = gameObject;
    LastPosition = transform.position;
    Speed = gameObject.GetComponent<PlayerMovable>().Speed;
  }

  void Update() {
    Vector3 instantaneousDirection = (transform.position - LastPosition) / Time.deltaTime;
    EstimatedDirection = 0.8f * instantaneousDirection + 0.2f * EstimatedDirection;
    LastPosition = transform.position;
  }
}
