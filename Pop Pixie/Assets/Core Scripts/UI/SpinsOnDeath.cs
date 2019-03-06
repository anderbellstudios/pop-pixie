using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinsOnDeath : MonoBehaviour {

  public float ZoomSpeed;
  public float SpinSpeed;
  public Camera Cam;

	// Update is called once per frame
	void Update () {
    if ( StateManager.Isnt( State.Dying ) )
      return;

    Cam.orthographicSize -= ZoomSpeed * Time.deltaTime;
    gameObject.transform.Rotate( 0, 0, SpinSpeed * Time.deltaTime );
	}
}
