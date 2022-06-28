using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level3GremlinPhase : APhase {

  public List<GremlinSpawnScheduler> GremlinSpawnSchedulers;

	public override void LocalBegin () {
    // LaserScheduler.enabled = true;
    GremlinSpawnSchedulers.ForEach( sch => StartGremlinSpawener(sch) );
  }

  void StartGremlinSpawener( GremlinSpawnScheduler sch ) {
    sch.BeginSpawning();
  }

  public override void WhilePhaseRunning() {
    bool all_finished = GremlinSpawnSchedulers.All( sch => !sch.Spawning );

    if ( all_finished ) {
      PhaseFinished();
    }
  }

}
