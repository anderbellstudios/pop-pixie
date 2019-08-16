using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GremlinSpawnScheduler : MonoBehaviour {

  public SpawnGremlin SpawnGremlin;
  public float SpawnInterval;
  public int MaxGremlins;

  private bool Spawning;
  private List<GameObject> SpawnedGremlins;

  private IntervalTimer SpawnTimer;

  public void BeginSpawning() {
    Spawning = true;
    SpawnedGremlins.Clear();

    SpawnTimer = new IntervalTimer() {
      Interval = SpawnInterval
    };
    
    TentativeSpawnNextGremlin();
  }

  void Update() {
    if ( !Spawning )
      return;

    if ( DestroyedLastGremlin() || SpawnTimer.Elapsed() )
      TentativeSpawnNextGremlin();
  }

  void TentativeSpawnNextGremlin() {
    if ( SpawnedGremlins.Count < MaxGremlins ) {
      var gremlin = SpawnGremlin.Spawn();
      SpawnedGremlins.Add(gremlin);
      SpawnTimer.Reset();
    }
  }

  bool DestroyedLastGremlin() {
    return SpawnedGremlins.Last() == null;
  }

}
