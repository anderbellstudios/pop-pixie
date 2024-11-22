using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineOfMovement : MonoBehaviour {
  public static bool Check(
    Vector3 start,
    Vector3 end,
    LayerMask? layerMask = null,
    GameObject exclude = null,
    Vector2? capsuleSize = null
  ) {
    Vector3 direction = end - start;
    Vector2 safeCapsuleSize = capsuleSize ?? new Vector2(1f, 1.5f);
    LayerMask safeLayerMask = layerMask ?? CollisionMask.UnwalkableMask;

    if (exclude) {
      return !Physics2D.CapsuleCastAll(
        origin: start,
        size: safeCapsuleSize,
        capsuleDirection: CapsuleDirection2D.Vertical,
        angle: 0f,
        direction: direction,
        distance: direction.magnitude,
        layerMask: safeLayerMask
      ).Where(hit => hit.collider.gameObject != exclude).FirstOrDefault();
    }

    return !Physics2D.CapsuleCast(
      origin: start,
      size: safeCapsuleSize,
      capsuleDirection: CapsuleDirection2D.Vertical,
      angle: 0f,
      direction: direction,
      distance: direction.magnitude,
      layerMask: safeLayerMask
    );
  }
}
