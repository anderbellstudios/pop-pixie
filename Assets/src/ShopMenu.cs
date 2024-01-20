using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : AMenu {
  public Image WeaponImage;
  public TMP_Text
    ActionButtonHintText,
    WeaponNameText,
    DescriptionText,
    DamageText, FireRateText, CapacityText, BulletSpeedText, ReloadTimeText, PriceText;
  public RectTransform ScrollContentArea;
  public ScrollRect ScrollRect;
  public PlayCaptionLine WelcomeLine, BuyLine, SellLine, CannotAffordLine, CannotSellFreeLine;
  public SceneChangeHopper ToTestingRoom;
  public PhaseScheduler LeaveShop;

  private bool Leaving = false;

  public override void LocalStartBeforeSelect() {
    Buttons.ForEach(button => {
      ShopWeaponTile weaponTile = button.gameObject.GetComponent<ShopWeaponTile>();

      weaponTile.ClickHandler = () => {
        if (Leaving)
          return;

        if (weaponTile.Bought)
          AttemptSell(weaponTile);
        else
          AttemptBuy(weaponTile);
      };
    });

    WelcomeLine.Perform();
  }

  void AttemptBuy(ShopWeaponTile weaponTile) {
    if (RingPullsData.Amount() < weaponTile.Price) {
      CannotAffordLine.Perform();
      return;
    }

    BuyOrSell(weaponTile, true);
    BuyLine.Perform();
  }

  void AttemptSell(ShopWeaponTile weaponTile) {
    if (weaponTile.Free) {
      CannotSellFreeLine.Perform();
      return;
    }

    BuyOrSell(weaponTile, false);
    SellLine.Perform();
  }

  void BuyOrSell(ShopWeaponTile weaponTile, bool buy) {
    RingPullsData.Modify(weaponTile.Price * (buy ? -1 : 1));
    BoughtWeaponsData.SetBought(weaponTile.Weapon.Id, buy);
    weaponTile.Bought = buy;
  }

  public void HandleSelectionChanged(GameObject currentSelected, GameObject previousSelected) {
    ShopWeaponTile weaponTile = currentSelected?.GetComponent<ShopWeaponTile>();

    if (weaponTile != null) {
      ScrollToSelectionHelper.EnsureVisible(
        targetTransform: (RectTransform)currentSelected.transform,
        contentArea: ScrollContentArea,
        scrollRect: ScrollRect
      );

      ShowWeaponInfo(weaponTile);
    }
  }

  public void ShowWeaponInfo(ShopWeaponTile weaponTile) {
    Weapon weapon = weaponTile.Weapon;

    ActionButtonHintText.text = (weaponTile.Bought ? "Sell" : "Buy") + " <size=150%>[Confirm]</size>";
    WeaponNameText.text = weaponTile.Name;
    DescriptionText.text = weaponTile.Description;
    PriceText.text = weaponTile.PriceString();

    if (weapon != null) {
      WeaponImage.sprite = weapon.Sprite;
      DamageText.text = weapon.Damage.ToString();
      FireRateText.text = weapon.FireRate.ToString();
      CapacityText.text = weapon.Capacity.ToString();
      BulletSpeedText.text = weapon.BulletSpeed == 0 ? "Not applicable" : weapon.BulletSpeed.ToString();
      ReloadTimeText.text = weapon.ReloadDuration.ToString();
    } else {
      WeaponImage.sprite = null;
      DamageText.text = "--";
      FireRateText.text = "--";
      CapacityText.text = "--";
      BulletSpeedText.text = "--";
      ReloadTimeText.text = "--";
    }
  }

  public override void LocalUpdate() {
    if (Leaving)
      return;

    if (WrappedInput.GetButtonDown("Reload")) {
      Leaving = true;
      ToTestingRoom.Hop();
    } else if (WrappedInput.GetButtonDown("Cancel")) {
      Leaving = true;
      LeaveShop.BeginFirstPhase();
    }
  }
}
