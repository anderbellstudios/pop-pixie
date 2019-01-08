using System.Collections;
using System.Collections.Generic;
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
// 6. And then try again with 20˚, 30˚, ..., 90˚.
// 7. Give up. There’s no simple two-step path to reach the player.
//
// One day, this will have its own Wikipedia article. I'm sure of it. 

public class AndersonsAlgorithm {

  Vector3 Start, Destination;
  float Radius; 

  public AndersonsAlgorithm(Vector3 start, Vector2 destination, float radius) {
    Start = start;
    Destination = destination;
    Radius = radius;
  }

  public Vector3[] Vertices() {
    // Cast a circle from the start to the destination
    RaycastHit2D[] hits = Physics2D.CircleCastAll( 
      Start, Radius, ToDestination()
    );

    Debug.DrawRay(Start, ToDestination(10), Color.blue, 5.0f);

    if ( DirectPath(hits) ) {
      Debug.Log("There is a direct path");
    } else {
      Debug.Log("There is no direct path");
    }

    return new Vector3[0];
  }

  private bool DirectPath(RaycastHit2D[] hits) {
    // If it hit anything other than itself
    if ( hits.Length > 1 ) {

      if ( ToDestination().magnitude - hits[1].distance > Radius * 2 ) {
        // Hit something before destination
        return false;
      } else {
        // Hit something at or after destination
        return true;
      }

    } else {
      return true;
    }
  }

  private Vector3 ToDestination(float angle = 0.0f) {
    return Quaternion.Euler(0, 0, angle) * (Destination - Start);
  }
}
