using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingNode : MonoBehaviour {
  public bool Dynamic = false;
  public PathfindingNode[] AdjacentNodes;

  void Awake() {
    RecomputeAdjacentNodes();
  }

  public void GraphWasRecomputed() {
    if (Dynamic) {
      RecomputeAdjacentNodes();
    }
  }

  void RecomputeAdjacentNodes() {
    AdjacentNodes = transform
      .parent
      .GetComponentsInChildren<PathfindingNode>()
      .Where(node => node != this && LineOfMovement.Check(transform.position, node.transform.position))
      .ToArray();
  }
}
