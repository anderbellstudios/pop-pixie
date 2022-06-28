using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesKilled : MonoBehaviour {

  public List<GameObject> Enemies;
  public bool Triggered;
  public LevelCompleted LevelCompleted;

	// Update is called once per frame
	void Update () {
    if (Triggered)
      return;

    bool all_dead = Enemies.All( x => EnemyUtils.IsDead(x) );

    if (all_dead) {
      Triggered = true;
      LevelCompleted.Run();
    }
	}
}
