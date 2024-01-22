using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * - Weapons are rendered on an infinite helix in 3D space.
 * - Near Z is 0.5, far Z is 1.5, center Z is 1.0.
 * - Item 0 has angle 0.
 */

public class SpiralWeaponSwitcher : AWeaponSwitcherUI {
  public GameObject BaseItem;
  public float AngleStepDegrees;
  public int BeforeAfterCount;
  public float Radius;
  public float Stretch;
  public float JoystickThreshold, CursorThreshold;

  private Vector2 PreviousDirection = Vector2.right;
  private float TargetAngle, CurrentAngle = 0f;
  private float AngleStep;
  private Dictionary<int, GameObject> ItemGameObjects = new Dictionary<int, GameObject>();
  private List<PlayerWeapon> Weapons;

  void Start() {
    AngleStep = Mathf.Deg2Rad * AngleStepDegrees;
  }

  public override void OnOpen(List<PlayerWeapon> availableWeapons, int equippedWeaponIndex) {
    Weapons = availableWeapons;
  }

  public override int OnClose() => WeaponIndexForItemIndex(
    ClosestItemIndexForAngle(TargetAngle)
  );

  void Update() {
    Vector2 direction = InputMode.IsJoystick()
      ? new Vector2(
        WrappedInput.GetAxis("Horizontal"),
        WrappedInput.GetAxis("Vertical")
      )
      : CursorDirection.DirectionFromScreenCenter() / (Screen.height / 2f);

    float threshold = InputMode.IsJoystick() ? JoystickThreshold : CursorThreshold;

    if (direction.magnitude > threshold) {
      float angle = Vector2.SignedAngle(PreviousDirection, direction) * Mathf.Deg2Rad;
      TargetAngle += angle;
      PreviousDirection = direction;
    }

    CurrentAngle = Mathf.Lerp(CurrentAngle, TargetAngle, 0.3f);

    int currentItemIndex = ClosestItemIndexForAngle(CurrentAngle);

    int firstItemIndex = currentItemIndex - BeforeAfterCount;
    int lastItemIndex = currentItemIndex + BeforeAfterCount;

    DestroyUnusedItems(minIndex: firstItemIndex, maxIndex: lastItemIndex);

    for (int i = firstItemIndex; i < lastItemIndex; i++) {
      GameObject item = GetOrCreateItem(i);

      float angle = AngleForItemIndex(i);

      Vector3 position = HelixPosition(
        radius: Radius,
        scaleZ: Stretch,
        centerAngle: CurrentAngle,
        angle: angle
      );

      item.transform.localPosition = Perspective(position);

      float scale = 1f / position.z;
      item.transform.localScale = Vector3.one * scale;

      SpiralWeaponTile weaponTile = item.GetComponent<SpiralWeaponTile>();

      weaponTile.SetOpacity(
        opacity: Mathf.Clamp01(1f - Mathf.Abs(position.z - 1f) * 2f),
        selected: i == currentItemIndex
      );
    }
  }

  float AngleForItemIndex(int index) => AngleStep * index;
  int ClosestItemIndexForAngle(float angle) => Mathf.RoundToInt(angle / AngleStep);

  GameObject GetOrCreateItem(int index) {
    if (ItemGameObjects.ContainsKey(index)) {
      return ItemGameObjects[index];
    }

    GameObject item = Instantiate(BaseItem, transform);
    item.SetActive(true);

    int weaponIndex = WeaponIndexForItemIndex(index);
    PlayerWeapon weapon = Weapons[weaponIndex];
    item.GetComponent<SpiralWeaponTile>().SetWeapon(weapon);

    ItemGameObjects[index] = item;

    return item;
  }

  void DestroyUnusedItems(int minIndex, int maxIndex) {
    foreach (int index in ItemGameObjects.Keys.ToArray()) {
      if (index < minIndex || index > maxIndex) {
        Destroy(ItemGameObjects[index]);
        ItemGameObjects.Remove(index);
      }
    }
  }

  Vector3 HelixPosition(float radius, float scaleZ, float centerAngle, float angle) => new Vector3(
    radius * Mathf.Cos(angle),
    radius * Mathf.Sin(angle),
    (scaleZ * (angle - centerAngle)) + 1f // 1.0 when centered
  );

  Vector3 Perspective(Vector3 p) => new Vector3(
    p.x / p.z,
    p.y / p.z,
    p.z // For ease of debugging
  );

  int WeaponIndexForItemIndex(int index) => PositiveMod(index, Weapons.Count);
  int PositiveMod(int x, int m) => (x % m + m) % m;
}
