using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlyingRingPull : MonoBehaviour {

  public GameObject Prefab;

  public int Amount = 1;
  public float Interval = 0.05f;

  Transform Parent; 

  int AlreadyDropped = 0;

  void Awake() {
    Parent = GameObject.Find("HUD").transform;
  }

  public void Instantiate() {
    InvokeRepeating("DropRingPull", 0, Interval);
  }

  void DropRingPull() {
    AlreadyDropped++;

    GameObject flyingRingPull = Instantiate(Prefab, Parent);
    flyingRingPull.transform.position = Camera.main.WorldToScreenPoint(transform.position);

    if ( AlreadyDropped >= Amount )
      CancelInvoke("DropRingPull");
  }

}
