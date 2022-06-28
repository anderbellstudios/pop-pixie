using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentoeRunToElevator : MonoBehaviour {

  public float Speed;
  public List<Vector3> Points;

  private bool Running;

  void Awake () {
    Running = false;
  }

  public void Run () {
    Running = true;
  }

  public void Skip () {
    Destroy( gameObject );
  }

  void Update () {
    if ( !Running )
      return;

    Vector3 dest = Points[0];

    transform.position = Vector3.MoveTowards(
      transform.position, 
      dest, 
      Speed * Time.deltaTime
    );

    float dist = ( transform.position - dest ).magnitude;

    if ( dist < 0.01 ) {
      Points.RemoveAt(0);
    }

    if ( Points.Count == 0 ) {
      Destroy( gameObject );
      Running = false;
    }

  }

}
