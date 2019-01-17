using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

  public MonoBehaviour CoolingDown;
  public MonoBehaviour Attacking;

  public float ActivationRadius;
  public double CoolDownDuration;
  public double GiveUpTime;

  // Making all attributes public for debugging purposes;
  // Should be changed later to avoid clutter in the inspector
  public bool Engaged;
  public DateTime LastActive;

  // Solely for debugging
  public double DebugCoolDownTimer;

	// Use this for initialization
	void Start () {
    Engaged = false;
    ResetCoolDownTimer();
    DisableAllAIs();
	}
	
	// Update is called once per frame
	void Update () {
    // Make the cool down appear in the inspector
    DebugCoolDownTimer = CoolDownTimer();

    if ( Engaged ) {
      var t = CoolDownTimer(); // Avoid repeat calls
      
      if (t < CoolDownDuration)
        SetSubAI( CoolingDown );
      if (t >= CoolDownDuration && t < GiveUpTime)
        SetSubAI( Attacking );
      if (t >= GiveUpTime)
        ResetCoolDownTimer();

    } else {

      DisableAllAIs(); // Unengaged

      if ( DistanceToTarget() < ActivationRadius ) {
        Engaged = true;
        ResetCoolDownTimer();
      }

    }
	}

  public void ResetCoolDownTimer() {
    LastActive = DateTime.Now;
  }

  private double CoolDownTimer() {
    // Seconds since last active
    return DateTime.Now.Subtract( LastActive ).TotalSeconds;
  }

  private float DistanceToTarget() {
    var target = GameObject.FindGameObjectWithTag("Player");

    if (target == null) {
      Debug.Log("Target not found!");
      return float.PositiveInfinity;
    }

    return Vector2.Distance(
      this.transform.position,
      target.transform.position
    );
  }

  private MonoBehaviour[] SubAIs() {
    // Array of all sub-AIs
    return new MonoBehaviour[] { 
      CoolingDown,
      Attacking
    };
  }

  private MonoBehaviour CurrentAI;

  private void SetSubAI(MonoBehaviour ai) {
    if ( ai == CurrentAI ) 
      return;
    
    Debug.Log("Starting new AI");
    Debug.Log(ai);

    DisableAllAIs();

    CurrentAI = ai;
    ai.enabled = true;
  }

  private void DisableAllAIs() {
    CurrentAI = null;

    foreach ( MonoBehaviour ai in SubAIs() ) {
      ai.enabled = false;
    }
  }
}
