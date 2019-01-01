using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

  public float ActivationRadius;
  // Making all attributes public for debugging purposes;
  // Should be changed later to avoid clutter in the inspector
  public bool Engaged;
  public DateTime LastActive;

	// Use this for initialization
	void Start () {
    Engaged = false;
	}
	
	// Update is called once per frame
	void Update () {
    Debug.Log("Clever AI procedure goes here!");
	}
}
