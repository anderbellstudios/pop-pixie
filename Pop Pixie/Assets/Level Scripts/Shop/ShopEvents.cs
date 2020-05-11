using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopEvents : GenericMenuEvents {

  public TMP_Text WeaponNameLabel;
  public TMP_Text BuySellLabel;

  private Maybe<WeaponTile> SelectedWeapon;

  void Update() {
    string weaponName = SelectedWeapon.Map(w => w.Name).GetOrDefault("");
    WeaponNameLabel.text = weaponName;

    bool bought = SelectedWeapon.Map(w => w.Bought).GetOrDefault(false);
    BuySellLabel.text = bought ? "Sell" : "Buy";
  }

  public void CeasePerusal() {
    FadeOut( () => {
      SaveGame.ReadSave();
      SceneData.Load();
    });
  }

  public void OnWeaponInteract( WeaponTile weaponTile ) {
    weaponTile.Bought = !weaponTile.Bought;
  }

  public void OnWeaponSelect( WeaponTile weaponTile ) {
    SelectedWeapon = Maybe<WeaponTile>.Some( weaponTile );
  }

  public void OnWeaponDeselect( WeaponTile weaponTile ) {
    SelectedWeapon = Maybe<WeaponTile>.None;
  }

}
