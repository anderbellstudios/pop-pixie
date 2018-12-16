using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitPoints : MonoBehaviour {

  public float Maximum; 
  public float Current; 

  public void Cap () {
    // Make sure HP is between 0 and max
    Current = Mathf.Clamp( Current, 0, Maximum );
  }

  public float Set (float val) {
    Current = val;
    Cap();
    return Current;
  }

  public float Increase (float val) {
    Current += val;
    Cap();
    return Current;
  }

  public float Decrease (float val) {
    return Increase(-val);
  }

	// Use this for initialization
	void Start () {
    Current = Maximum;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
