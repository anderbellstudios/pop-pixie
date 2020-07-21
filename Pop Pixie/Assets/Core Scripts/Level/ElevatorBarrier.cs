using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBarrier : MonoBehaviour {

  public EdgeCollider2D Collider;

  public void Remove() {
    Collider.enabled = false;
	}
}
