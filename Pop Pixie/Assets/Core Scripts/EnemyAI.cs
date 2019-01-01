using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

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
	}
	
	// Update is called once per frame
	void Update () {
    // Make the cool down appear in the inspector
    DebugCoolDownTimer = CoolDownTimer();

    if ( Engaged ) {
      var t = CoolDownTimer(); // Avoid repeat calls
      
      if (t < CoolDownDuration)
        Debug.Log("I am cooling down");
      if (t >= CoolDownDuration && t < GiveUpTime)
        Debug.Log("I am attacking");
      if (t >= GiveUpTime)
        ResetCoolDownTimer();

    } else {

      if ( DistanceToTarget() < ActivationRadius ) {
        Engaged = true;
        ResetCoolDownTimer();
      } else {
        Debug.Log( DistanceToTarget() );
      }

    }
	}

  private double CoolDownTimer() {
    // Seconds since last active
    return DateTime.Now.Subtract( LastActive ).TotalSeconds;
  }

  private void ResetCoolDownTimer() {
    LastActive = DateTime.Now;
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
}
