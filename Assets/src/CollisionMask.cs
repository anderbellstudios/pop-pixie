using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionMask {
  private static Dictionary<int, LayerMask> Cache = new();

  public static LayerMask ForLayer(int ownLayer) {
    if (Cache.ContainsKey(ownLayer))
      return Cache[ownLayer];

    LayerMask mask = 0;

    for (int otherLayer = 0; otherLayer < 32; otherLayer++) {
      if (!Physics2D.GetIgnoreLayerCollision(ownLayer, otherLayer)) {
        mask |= 1 << otherLayer;
      }
    }

    Cache.Add(ownLayer, mask);

    return mask;
  }

  private static LayerMask? _UnwalkableMask;
  public static LayerMask UnwalkableMask => (
    _UnwalkableMask ?? (_UnwalkableMask = GetUnwalkableMask())
  ).Value;

  private static LayerMask GetUnwalkableMask() =>
    LayerMask.GetMask("Default") |
    LayerMask.GetMask("RollToPass") |
    LayerMask.GetMask("CrouchToPass") |
    LayerMask.GetMask("WalkBoundary") |
    LayerMask.GetMask("TransparentWall");

  private static LayerMask? _OpaqueMask;
  public static LayerMask OpaqueMask => (
    _OpaqueMask ?? (_OpaqueMask = GetOpaqueMask())
  ).Value;

  private static LayerMask GetOpaqueMask() =>
    LayerMask.GetMask("Default") |
    LayerMask.GetMask("CrouchToPass");

  private static LayerMask? _PlayerMask;
  public static LayerMask PlayerMask => (
    _PlayerMask ?? (_PlayerMask = GetPlayerMask())
  ).Value;

  private static LayerMask GetPlayerMask() =>
    LayerMask.GetMask("Player") |
    LayerMask.GetMask("PlayerRolling") |
    LayerMask.GetMask("PlayerCrouching");

  private static LayerMask? _BulletMask;
  public static LayerMask BulletMask => (
    _BulletMask ?? (_BulletMask = GetBulletMask())
  ).Value;

  private static LayerMask GetBulletMask() =>
    LayerMask.GetMask("GenericBullet") |
    LayerMask.GetMask("PlayerBullet") |
    LayerMask.GetMask("EnemyBullet");
}
