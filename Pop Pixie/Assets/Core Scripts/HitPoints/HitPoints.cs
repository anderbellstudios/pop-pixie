using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

class HitPointEventsFacade : IHitPointEvents {
  private List<IHitPointEvents> EventHandlers;

  public HitPointEventsFacade(List<IHitPointEvents> eventHandlers) {
    EventHandlers = eventHandlers;
  }

  public void Updated(HitPoints hp)
    => EventHandlers.ForEach(x => x.Updated(hp));

  public void Decreased(HitPoints hp)
    => EventHandlers.ForEach(x => x.Decreased(hp));

  public void BecameZero(HitPoints hp)
    => EventHandlers.ForEach(x => x.BecameZero(hp));
}

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
  public bool InfiniteHP = false;
  public float Current; 
  public float RegenerateRate;
  public List<ACanBeDamagedArbiter> CanBeDamagedArbiters;
  public DateTime LastDamaged;
  public bool Dead = false;

  private IHitPointEvents EventHandler;

  public void Cap () {
    // Make sure HP is between 0 and max
    Current = Mathf.Clamp( Current, 0, Maximum );
  }

  public float Set (float val) {
    Current = val;
    Cap();
    EventHandler.Updated(this);
    return Current;
  }

  public float Increase (float val) {
    Current += val;
    Cap();
    EventHandler.Updated(this);
    return Current;
  }

  public float Decrease (float val) {
    if ( Dead )
      return 0.0f; // <-- Bypass callbacks

    if (!InfiniteHP)
      Increase(-val);

    EventHandler.Decreased(this);
    if ( Current == 0 ) {
      Dead = true;
      EventHandler.BecameZero(this);
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
    EventHandler = new HitPointEventsFacade(
      gameObject.GetComponents<IHitPointEvents>().ToList()
    );

    GDCall.UnlessLoad( InitHitPoints );

    if ( !ShouldSave )
      InitHitPoints();

    EventHandler.Updated(this);

    if (Current == 0) {
      Dead = true;
      EventHandler.BecameZero(this);
    }
	}

  public void InitHitPoints() {
    Current = Maximum;
    EventHandler.Updated(this);
  }
	
	// Update is called once per frame
	void Update () {
    if ( StateManager.Is( State.Playing ) )
      Increase( RegenerateRate * Time.deltaTime );
	}

}
