using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleWeaponDirectionManager : MonoBehaviour, IDirectionManager {

  public Vector3 Direction { get; set; }

  public GameObject Arrow;

  public JoystickWeaponDirectionManager JoystickWeaponDirectionManager;
  public CursorWeaponDirectionManager CursorWeaponDirectionManager;

  private IDirectionManager ActiveDirectionManager;

  // Use this for initialization
  void Start () {
    Direction = Direction.normalized;
  }
  
  // Update is called once per frame
  void Update () {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( CursorWeaponDirectionManager.Active() || ActiveDirectionManager == null ) {
      ActiveDirectionManager = CursorWeaponDirectionManager;
    } else if ( JoystickWeaponDirectionManager.Active() ) {
      ActiveDirectionManager = JoystickWeaponDirectionManager;
    }

    var newDirection = ActiveDirectionManager.Direction;

    if ( newDirection.magnitude > 0 ) {
      Direction = newDirection;

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

}
