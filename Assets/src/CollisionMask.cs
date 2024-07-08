using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionMask {
  private static Dictionary<int, int> Cache = new Dictionary<int, int>();

  public static int ForLayer(int ownLayer) {
    if (Cache.ContainsKey(ownLayer))
      return Cache[ownLayer];

    int mask = 0;

    for (int otherLayer = 0; otherLayer < 32; otherLayer++) {
      if (!Physics2D.GetIgnoreLayerCollision(ownLayer, otherLayer)) {
        mask |= 1 << otherLayer;
      }
    }

    Cache.Add(ownLayer, mask);

    return mask;
  }
}
