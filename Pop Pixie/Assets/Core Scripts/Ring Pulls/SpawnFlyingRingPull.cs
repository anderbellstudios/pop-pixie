using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlyingRingPull : MonoBehaviour {

  public GameObject Prefab;
  public Transform Parent; 
  public Camera Camera;

  public int Amount = 1;
  public float Interval = 0.05f;

  int AlreadyDropped = 0;

  public void Instantiate() {
    InvokeRepeating("DropRingPull", 0, Interval);
  }

  void DropRingPull() {
    AlreadyDropped++;

    GameObject flyingRingPull = Instantiate(Prefab, Parent);
    flyingRingPull.transform.position = Camera.WorldToScreenPoint(transform.position);

    if ( AlreadyDropped >= Amount )
      CancelInvoke("DropRingPull");
  }

}
