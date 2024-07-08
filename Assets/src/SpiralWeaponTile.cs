using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpiralWeaponTile : MonoBehaviour {
  public Image Sprite;
  public TMP_Text Name;

  public void SetWeapon(PlayerWeapon weapon) {
    Sprite.sprite = weapon.Sprite;
    Name.text = weapon.Name;
  }

  public void SetOpacity(float opacity, bool selected) {
    Sprite.color = new Color(1f, 1f, 1f, opacity);
    Name.color = new Color(1f, 1f, selected ? 0f : 1f, opacity);
  }
}
