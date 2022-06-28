using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[ExecuteInEditMode]
public class WeaponTile : MonoBehaviour, ISelectHandler, IDeselectHandler {

  public string Name = "Untitled weapon";
  public Weapon Weapon;
  public bool InStock = false;
  public Sprite UnboughtSprite;
  public Sprite BoughtSprite;
  public int Price;

  [TextArea]
  public string Description;

  public bool Bought;

  public Image WeaponImage;
  public TMP_Text PriceLabel;

  void Awake() {
    if (InStock) {
      PriceLabel.text = Price.ToString();
    } else {
      PriceLabel.text = "--";
    }

    // If the weapon is null, Bought is false
    Bought = Maybe<Weapon>.ofNullable(Weapon)
      .Map( w => BoughtWeaponsData.IsBought(w.Id) )
      .GetOrDefault(false);

    UpdateSprite();
  }

  void UpdateSprite() {
    WeaponImage.sprite = Bought ? BoughtSprite : UnboughtSprite;
  }

  public void Interact() {
    MaybeShopEvents.If( shopEvents => shopEvents.InteractWithWeapon(this) );
    UpdateSprite();
  }

  public void OnSelect(BaseEventData eventData) {
    MaybeShopEvents.If( shopEvents => shopEvents.OnWeaponSelect(this) );
  }

  public void OnDeselect(BaseEventData eventData) {
    MaybeShopEvents.If( shopEvents => shopEvents.OnWeaponDeselect(this) );
  }

  Maybe<ShopEvents> MaybeShopEvents
    => Maybe<ShopEvents>.ofNullable(
      GameObject.Find("ShopEvents")?.GetComponent<ShopEvents>()
    );

}
