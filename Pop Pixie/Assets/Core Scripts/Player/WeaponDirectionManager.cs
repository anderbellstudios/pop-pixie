using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDirectionManager : MonoBehaviour {

  public Vector3 Direction;
  public float Threshold;
  public GameObject Arrow;
  public Camera camera;

  // Use this for initialization
  void Start () {
    Direction = Direction.normalized;
  }
  
  // Update is called once per frame
  void Update () {
    Vector3? inputDirection = JoystickDirection() ?? MouseDirection();

    if (inputDirection == null) {
      return;
    } else {
      Direction = (Vector3)inputDirection;
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

  Vector3? JoystickDirection() {
    float x_component = Input.GetAxis("Fire X");
    float y_component = Input.GetAxis("Fire Y");

    var inputDirection = new Vector3(
      x_component, 
      y_component, 
      0
    );

    if ( inputDirection.magnitude >= Threshold ) {
      return inputDirection.normalized;
    } else {
      return null;
    }
  }

  Vector3? MouseDirection() {
    Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
    
    var inputDirection = new Vector3(
      mousePosition.x - transform.position.x, 
      mousePosition.y - transform.position.y, 
      0
    );

    return inputDirection.normalized;
  }
}
