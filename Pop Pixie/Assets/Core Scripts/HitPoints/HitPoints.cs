using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class HitPoints : MonoBehaviour, ISerializableComponent {

  public string[] SerializableFields {
    get {
      List<string> fields = new List<string>();

      if (ShouldSave) {
        fields.Add("Current");
      }

      return fields.ToArray();
    }
  }

  public bool ShouldSave = true;
  public float Maximum; 
  public float Current; 
  public float RegenerateRate;
  public List<ACanBeDamagedArbiter> CanBeDamagedArbiters;

  public DateTime LastDamaged;

  private IHitPointEvents[] EventHandlers;

  public bool Dead = false;

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
    if ( Dead )
      return 0.0f; // <-- Bypass callbacks

    Increase(-val);

    SendEventHandlerMessage("Decreased");
    if ( Current == 0 ) {
      Dead = true;
      SendEventHandlerMessage("BecameZero");
    }

    return Current;
  }

  bool CanBeDamaged () {
    return CanBeDamagedArbiters.ToArray().All(
      arbiter => arbiter.CanBeDamaged(this)
    );
  }

  public float Damage (float val) {
    if ( CanBeDamaged() ) {
      LastDamaged = DateTime.Now;
      return Decrease(val);
    }

    return -1.0f;
  }

	// Use this for initialization
	void Start () {
    GDCall.UnlessLoad( InitHitPoints );

    if ( !ShouldSave )
      InitHitPoints();

    EventHandlers = gameObject.GetComponents<IHitPointEvents>();

    if (Current == 0) {
      Dead = true;
      SendEventHandlerMessage("BecameZero");
    }
	}

  public void InitHitPoints() {
    Current = Maximum;
  }
	
	// Update is called once per frame
	void Update () {
    SendEventHandlerMessage("Updated");

    if ( StateManager.Is( State.Playing ) )
      Increase( RegenerateRate * Time.deltaTime );
	}

  void SendEventHandlerMessage(string message) {
    foreach (IHitPointEvents eventHandler in EventHandlers) {
      MethodInfo method = eventHandler.GetType().GetMethod(message);
      method.Invoke(eventHandler, new object[] { this } );
    }
  }

}
