using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionTowards : MonoBehaviour, IDirectionManager {

  public Vector3 Direction { get; set; }

  public Transform Target;
  public string TargetName;

  void Awake() {
    if (Target == null)
      Target = GameObject.Find(TargetName).transform;
  }

  void Update() {
    Direction = Target.position - transform.position;
  }

}
