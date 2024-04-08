using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingGraph : MonoBehaviour {
  public static PathfindingGraph Current;
  public int RecomputeVersion = 0;

  private PathfindingNode[] PathfindingNodes;

  void Awake() {
    Current = this;
    PathfindingNodes = GetComponentsInChildren<PathfindingNode>();
  }

  public void Recompute() {
    RecomputeVersion++;
    foreach (PathfindingNode node in PathfindingNodes) {
      node.GraphWasRecomputed();
    }
  }

  public List<Vector3> FindPath(Vector3 startPoint, Vector3 endPoint) {
    if (LineOfMovement.Check(startPoint, endPoint)) {
      return new List<Vector3> { startPoint, endPoint };
    }

    PathfindingNode startNode = NearestNode(startPoint);
    PathfindingNode endNode = NearestNode(endPoint);

    if (!startNode || !endNode)
      return null;

    List<Vector3> path = FindPath(startNode, endNode)?
      .Select(node => node.transform.position)?
      .ToList();

    if (path == null)
      return null;

    path.Prepend(startPoint);
    path.Add(endPoint);

    return path;
  }

  public List<PathfindingNode> FindPath(PathfindingNode startNode, PathfindingNode endNode) {
    List<PathfindingNode> openSet = new List<PathfindingNode>() { startNode };
    Dictionary<PathfindingNode, PathfindingNode> cameFrom = new Dictionary<PathfindingNode, PathfindingNode>();

    Dictionary<PathfindingNode, float> gScore = new Dictionary<PathfindingNode, float>();
    gScore[startNode] = 0f;

    Dictionary<PathfindingNode, float> fScore = new Dictionary<PathfindingNode, float>();
    fScore[startNode] = Distance(startNode, endNode);

    while (openSet.Count > 0) {
      PathfindingNode currentNode = openSet.OrderBy(node => fScore[node]).First();

      if (currentNode == endNode) {
        List<PathfindingNode> path = new List<PathfindingNode>() { currentNode };

        while (cameFrom.ContainsKey(currentNode)) {
          currentNode = cameFrom[currentNode];
          path.Add(currentNode);
        }

        path.Reverse();
        return path;
      }

      openSet.Remove(currentNode);

      foreach (PathfindingNode adjacentNode in currentNode.AdjacentNodes) {
        float tentativeGScore = gScore[currentNode] + Distance(currentNode, adjacentNode);

        if (tentativeGScore < gScore.GetValueOrDefault(adjacentNode, float.PositiveInfinity)) {
          cameFrom[adjacentNode] = currentNode;
          gScore[adjacentNode] = tentativeGScore;
          fScore[adjacentNode] = gScore[adjacentNode] + Distance(adjacentNode, endNode);

          if (!openSet.Contains(adjacentNode)) {
            openSet.Add(adjacentNode);
          }
        }
      }
    }

    return null;
  }

  private float Distance(PathfindingNode a, PathfindingNode b) {
    return Vector3.Distance(a.transform.position, b.transform.position);
  }

  private PathfindingNode NearestNode(Vector3 point) {
    return PathfindingNodes
      .Where(node => LineOfMovement.Check(point, node.transform.position))
      .OrderBy(node => Vector3.Distance(point, node.transform.position))
      .FirstOrDefault();
  }
}
