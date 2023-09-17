using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Anderson’s Heuristic Pathfinding Algorithm:
// 1. Fire a circle cast towards the player. If it hits, then go that way.
// 2. If it doesn’t hit, then fire another circle cast at an angle of 10˚ 
//    to the first.
// 3. For every 1 unit length on this path, fire another circle cast 
//    towards the player.
// 4. If it hits, then go to the intermediate vertex and then towards the 
//    player.
// 5. If no intermediate points result in a valid path, then try 10˚ the 
//    other way.
// 6. And then try again with 20˚, 30˚, ..., 180˚.
// 7. Give up. There’s no simple two-step path to reach the player.
//
// One day, this will have its own Wikipedia article. I'm sure of it. 

public class AndersonsAlgorithm {

  static float[] Angles = new float[] {
    10, -10,
    20, -20,
    30, -30,
    40, -40,
    50, -50,
    60, -60,
    70, -70,
    80, -80,
    90, -90,
    100, -100
  };

  Vector3 Start, Destination;
  float Radius;
  int RemainingSteps;

  public AndersonsAlgorithm(Vector3 start, Vector2 destination, float radius, int remainingSteps = 1) {
    Start = start;
    Destination = destination;
    Radius = radius;
    RemainingSteps = remainingSteps;
  }

  public Vector3? NextPoint() {
    var hit = ObstacleInDirection(ToDestination());
    Debug.DrawRay(Start, ToDestination(), Color.blue, 5.0f);

    if (DirectPath(hit)) {
      return Destination;
    } else {
      return TryThroughElbow();
    }

  }

  private Vector3? TryThroughElbow() {
    // Stop if out of steps
    if (RemainingSteps == 0)
      return null;

    foreach (float angle in Angles) {

      var direction = ToDestination(angle);
      Debug.DrawRay(Start, direction, Color.red, 5.0f);

      // Get distance to nearest obstacle in this direction
      var hit = ObstacleInDirection(direction);
      float maxDist = (hit.collider != null) ? hit.distance : 20.0f;

      float dist = 1.0f;

      while (dist < maxDist) {
        // Find elbow position dist from start in direction
        var elbow = Start + (dist * Vector3.Normalize(direction));

        // In order to understand recursion...
        var pathfinder = new AndersonsAlgorithm(elbow, Destination, Radius, RemainingSteps - 1);

        if (pathfinder.NextPoint() != null) {
          return elbow;
        }

        // No route found thorugh this elbow.
        // Increment distance and try again.
        dist += 1.0f;
      }

    }

    // No route found through any elbow. 
    // Give up.
    return null;
  }

  private RaycastHit2D ObstacleInDirection(Vector3 direction) {
    // Cast a circle from the start to the destination
    return Physics2D.CircleCast(
      Start,
      Radius,
      direction,
      Mathf.Infinity,
      ~((1 << 8) | (1 << 9)) // <-- neither 8 nor 9
    );
  }

  private bool DirectPath(RaycastHit2D hit) {
    // If it hit anything other than itself
    if (hit.collider) {

      if (ToDestination().magnitude - hit.distance > Radius * 2) {
        // Hit something before destination
        return false;
      } else {
        // Hit something at or after destination
        return true;
      }

    } else {
      // Nothing was hit
      return true;
    }
  }

  private Vector3 ToDestination(float angle = 0.0f) {
    return Quaternion.Euler(0, 0, angle) * (Destination - Start);
  }
}
