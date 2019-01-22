using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour {

  public float Maximum; 
  public float Current; 

  private IHitPointEvents EventHandler;

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
    Increase(-val);

    EventHandler.Decreased(this);
    if ( Current == 0 )
      EventHandler.BecameZero(this);

    return Current;
  }

	// Use this for initialization
	void Start () {
    Current = Maximum;
    EventHandler = gameObject.GetComponent<IHitPointEvents>();
	}
	
	// Update is called once per frame
	void Update () {
    EventHandler.Updated(this);
	}
}
