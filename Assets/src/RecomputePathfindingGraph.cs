using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecomputePathfindingGraph : MonoBehaviour {
  public void Invoke() {
    PathfindingGraph.Recompute();
  }
}
