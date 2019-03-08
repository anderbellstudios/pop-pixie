using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesKilled : MonoBehaviour {

  public List<GameObject> Enemies;
  public bool Triggered;
  public EdgeCollider2D ElevatorBarrier;

	// Update is called once per frame
	void Update () {
    if (Triggered)
      return;

    bool all_dead = Enemies.All(
      enemy => enemy == null
    );

    if (all_dead) {
      Triggered = true;
      ElevatorBarrier.enabled = false;
    }
	}
}
