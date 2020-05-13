using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopEvents : GenericMenuEvents {

  public float DescriptionInterval;
  public float ClosingDelay;

  [TextArea]
  public string OpeningText, ClosingText, BuyText, SellText, OutOfStockText, CannotAffordText;

  public TMP_Text WeaponNameLabel;
  public TMP_Text BuySellLabel;
  public Captions Captions;

  private Maybe<WeaponTile> SelectedWeapon;
  private IntervalTimer DescriptionTimer;

  void Awake() {
    DescriptionTimer = new IntervalTimer() {
      Interval = DescriptionInterval
    };
  }

  public override void LocalStart() {
    Captions.SetText(OpeningText);
  }

  void Update() {
    string weaponName = SelectedWeapon.Map(w => w.Name).GetOrDefault("");
    WeaponNameLabel.text = weaponName;

    bool bought = SelectedWeapon.Map(w => w.Bought).GetOrDefault(false);
    BuySellLabel.text = bought ? "Sell" : "Buy";

    DescriptionTimer.IfElapsed( delegate {
      DescriptionTimer.Stop();
      SelectedWeapon.If( w => Describe(w) );
    });
  }

  public void CeasePerusal() {
    Captions.SetText(ClosingText);

    FadeOut( () => {
      SaveGame.WriteSave();
      SceneData.Load();
    });
  }

  bool FirstTime = true;

  public void OnWeaponSelect( WeaponTile weaponTile ) {
    SelectedWeapon = Maybe<WeaponTile>.Some( weaponTile );

    if ( FirstTime ) {
      // Don't interrupt the opening dialogue
      FirstTime = false;
      return;
    }

    DescriptionTimer.Reset();
  }

  public void OnWeaponDeselect( WeaponTile weaponTile ) {
    SelectedWeapon = Maybe<WeaponTile>.None;
    DescriptionTimer.Stop();
    Captions.ClearText();
  }

  public void InteractWithWeapon( WeaponTile weaponTile ) {
    DescriptionTimer.Stop();

    if (weaponTile.Bought) {
      Sell(weaponTile);
    } else {
      AttemptBuy(weaponTile);
    }
  }

  enum Outcome { Buy, OutOfStock, CannotAfford };

  void AttemptBuy( WeaponTile weaponTile ) {
    Outcome outcome;

    if ( CanBuy(weaponTile, out outcome) ) 
      Buy(weaponTile);

    switch (outcome) {
      case Outcome.Buy:
        Captions.SetText(BuyText);
        break;

      case Outcome.OutOfStock:
        Captions.SetText(OutOfStockText);
        break;

      case Outcome.CannotAfford:
        Captions.SetText(CannotAffordText);
        break;
    }
  }

  bool CanBuy( WeaponTile weaponTile, out Outcome outcome ) {
    if (!weaponTile.InStock) {
      outcome = Outcome.OutOfStock;
      return false;
    }

    if ( RingPullsData.Amount() < weaponTile.Price ) {
      outcome = Outcome.CannotAfford;
      return false;
    }

    outcome = Outcome.Buy;
    return true;
  }

  void Buy(WeaponTile weaponTile) {
    weaponTile.Bought = true;
    SetBoughtData(weaponTile, true);
    RingPullsData.Modify(-1 * weaponTile.Price);
  }

  void Sell(WeaponTile weaponTile) {
    weaponTile.Bought = false;
    SetBoughtData(weaponTile, false);
    RingPullsData.Modify(weaponTile.Price);
    Captions.SetText(SellText);
  }

  void SetBoughtData( WeaponTile weaponTile, bool value ) {
    BoughtWeaponsData.SetBought(weaponTile.Weapon.Id, value);
  }

  void Describe(WeaponTile weaponTile) {
    Captions.SetText(weaponTile.Description);
  }

}
