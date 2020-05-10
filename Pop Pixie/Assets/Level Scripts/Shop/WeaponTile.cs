using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class WeaponTile : MonoBehaviour {

  public Sprite Sprite;
  public int Price;
  public bool Bought;

  public Image WeaponImage;
  public TMP_Text PriceLabel;
  public MonoBehaviour TickImage;

  void Update() {
    WeaponImage.sprite = Sprite;
    PriceLabel.text = Price.ToString();

    byte opacity = (byte) (Bought ? 64 : 255);
    WeaponImage.color = new Color32(255, 255, 255, opacity);
    TickImage.enabled = Bought;
  }

  public void ToggleBought_TESTING_ONLY() {
    Bought = !Bought;
  }

}
