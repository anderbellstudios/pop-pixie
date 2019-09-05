using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GremlinSpawnScheduler : MonoBehaviour {

  public SpawnGremlin SpawnGremlin;
  public float SpawnInterval;
  public int MaxGremlins;

  public bool Spawning;
  public List<GameObject> SpawnedGremlins;

  private IntervalTimer SpawnTimer;

  public void BeginSpawning() {
    SpawnedGremlins.Clear();
    Spawning = true;

    SpawnTimer = new IntervalTimer() {
      Interval = SpawnInterval
    };
    
    TentativeSpawnNextGremlin();
  }

  void Update() {
    if ( !Spawning )
      return;

    if ( DestroyedAllGremlins() || SpawnTimer.Elapsed() )
      TentativeSpawnNextGremlin();

    if ( SpawnedGremlins.Count == MaxGremlins && DestroyedAllGremlins() )
      Spawning = false;
  }

  void TentativeSpawnNextGremlin() {
    if ( SpawnedGremlins.Count < MaxGremlins ) {
      var gremlin = SpawnGremlin.Spawn();
      SpawnedGremlins.Add(gremlin);
      SpawnTimer.Reset();
    }
  }

  bool DestroyedAllGremlins() {
    return SpawnedGremlins.All( x => x == null);
  }

}
