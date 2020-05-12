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
  public Sprite Sprite;
  public int Price;
  public bool Bought;

  public Image WeaponImage;
  public TMP_Text PriceLabel;
  public MonoBehaviour TickImage;

  void Awake() {
    WeaponImage.sprite = Sprite;
    PriceLabel.text = Price.ToString();

    // If the weapon is null, Bought is false
    Bought = Maybe<Weapon>.ofNullable(Weapon)
      .Map( w => BoughtWeaponsData.IsBought(w.Id) )
      .GetOrDefault(false);

    UpdateBoughtIndicator();
  }

  void UpdateBoughtIndicator() {
    byte opacity = (byte) (Bought ? 64 : 255);
    WeaponImage.color = new Color32(255, 255, 255, opacity);
    TickImage.enabled = Bought;
  }

  public void Interact() {
    if (Bought) {
      Sell();
    } else {
      AttemptBuy();
    }

    UpdateBoughtIndicator();
  }

  void AttemptBuy() {
    string reason = "";

    if ( CanBuy(out reason) ) {
      Buy();
    } else {
      Debug.Log(reason);
    }
  }

  bool CanBuy(out string reason) {
    if (!InStock) {
      reason = "That's out of stock.";
      return false;
    }

    if ( RingPullsData.Amount() < Price ) {
      reason = "You can't afford it.";
      return false;
    }

    reason = "";
    return true;
  }

  void Buy() {
    Bought = true;
    SetBoughtData(true);
    RingPullsData.Modify(-1 * Price);
  }

  void Sell() {
    Bought = false;
    SetBoughtData(false);
    RingPullsData.Modify(Price);
  }

  void SetBoughtData(bool value) {
    BoughtWeaponsData.SetBought(Weapon.Id, value);
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
