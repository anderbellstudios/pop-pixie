using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineOfMovement : MonoBehaviour {
  public static bool Check(Vector3 start, Vector3 end, int? layerMask = null, GameObject exclude = null) {
    Vector3 direction = end - start;

    RaycastHit2D hit = Physics2D.CircleCastAll(
      origin: start,
      radius: 0.5f,
      direction: direction,
      distance: direction.magnitude,
      layerMask: layerMask ?? LayerMask.GetMask("Default")
    ).Where(hit => hit.collider.gameObject != exclude).FirstOrDefault();

    return !hit.collider;
  }
}
