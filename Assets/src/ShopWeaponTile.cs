using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class ShopWeaponTile : MonoBehaviour {
  public Weapon Weapon;
  public int Price;
  [TextArea] public string Description;
  public Image Image, DarkenImage, TickImage;
  public TMP_Text NameLabel, PriceLabel;

  private bool _Bought;
  public bool Bought {
    get { return _Bought; }
    set {
      _Bought = value;
      HandleBoughtChanged();
    }
  }

  public Action ClickHandler;

  void Awake() {
    NameLabel.text = Weapon.Name;
    Image.sprite = Weapon.Sprite;
    PriceLabel.text = PriceString();
    Bought = Price == -1 || BoughtWeaponsData.IsBought(Weapon.Id);
  }

  void HandleBoughtChanged() {
    DarkenImage.enabled = TickImage.enabled = Bought;
  }

  public void HandleClick() {
    ClickHandler.Invoke();
  }

  public bool Free => Price <= 0;
  public string PriceString() => Free ? "Free" : Price.ToString() + " <sprite=\"Ring Pull Icon\" name=\"Ring Pull\">";
}
