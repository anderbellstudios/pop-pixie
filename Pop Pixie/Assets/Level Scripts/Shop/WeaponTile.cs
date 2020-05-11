using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[ExecuteInEditMode]
public class WeaponTile : MonoBehaviour, ISelectHandler, IDeselectHandler {

  public string Name = "Untitled weapon";
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

  public void Interact() {
    if (Bought) {
      Sell();
    } else {
      Buy();
    }
  }

  void Buy() {
    if ( RingPullsData.Amount() >= Price ) {
      Bought = true;
      RingPullsData.Modify(-1 * Price);
    } else {
      Debug.Log("You can't afford it.");
    }
  }

  void Sell() {
    Bought = false;
    RingPullsData.Modify(Price);
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
