using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDirectionManager : MonoBehaviour {

  public Vector3 Direction;
  public float Threshold;
  public GameObject Arrow;

  // Use this for initialization
  void Start () {
    Direction = Direction.normalized;
  }
  
  // Update is called once per frame
  void Update () {
    float x_component = Input.GetAxis("Fire X");
    float y_component = Input.GetAxis("Fire Y");

    var inputDirection = new Vector3(
      x_component, 
      y_component, 
      0
    );

    if ( inputDirection.magnitude >= Threshold ) {
      Direction = inputDirection.normalized;
    }

    var rotation = Quaternion.FromToRotation(
      new Vector3( 0, 1, 0 ),
      Direction
    );

    Arrow.transform.rotation = Quaternion.Slerp(
      Arrow.transform.rotation,
      rotation,
      0.3f
    );
  }
}
