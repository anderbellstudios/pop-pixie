using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDirectionManager : MonoBehaviour {

  public Vector3 Direction;
  public float JoystickThreshold;
  public float MouseThreshold;
  public GameObject Arrow;
  public Camera Cam;

  // Use this for initialization
  void Start () {
    Direction = Direction.normalized;
  }
  
  // Update is called once per frame
  void Update () {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    Vector3? inputDirection = JoystickDirection() ?? MouseDirection();

    if (inputDirection != null) {
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

    if ( inputDirection.magnitude >= JoystickThreshold ) {
      return inputDirection.normalized;
    } else {
      return null;
    }
  }

  private Vector3 previousMousePosition = Vector3.zero;

  Vector3? MouseDirection() {
    var mousePosition = Input.mousePosition;

    float movementFactor = (previousMousePosition - mousePosition).magnitude;

    previousMousePosition = mousePosition;

    if ( movementFactor >= MouseThreshold ) {
      var onscreenMousePosition = Cam.ScreenToWorldPoint(mousePosition);

      var inputDirection = new Vector3(
        onscreenMousePosition.x - transform.position.x, 
        onscreenMousePosition.y - transform.position.y, 
        0
      );

      return inputDirection.normalized;

    } else {
      return null;
    }

  }
}
