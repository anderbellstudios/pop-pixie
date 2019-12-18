using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlyingRingPull : MonoBehaviour {

  public GameObject Prefab;
  public Transform Parent; 
  public Camera Camera;

  public void Instantiate() {
    GameObject flyingRingPull = Instantiate(Prefab, Parent);
    flyingRingPull.transform.position = Camera.WorldToScreenPoint(transform.position);
  }

}
