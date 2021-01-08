using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePath : MonoBehaviour {

  public float Speed;
  public List<Vector3> Points;
  public bool Running;
  public bool LoopInfinitely;

  private int PointIndex = 0;

  void Update() {
    if (StateManager.Isnt(State.Playing) || !Running)
      return;

    Vector3 dest = Points[PointIndex];

    transform.localPosition = Vector3.MoveTowards(
      transform.localPosition, 
      dest, 
      Speed * Time.deltaTime
    );

    float dist = (transform.localPosition - dest).magnitude;

    if (dist < 0.01) {
      PointIndex++;
    }

    if (PointIndex >= Points.Count) {
      if (LoopInfinitely) {
        PointIndex = 0;
      } else {
        Running = false;
      }
    }

  }

}
