using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopEvents : GenericMenuEvents {

  public void CeasePerusal() {
    FadeOut( () => {
      SaveGame.ReadSave();
      SceneData.Load();
    });
  }

  public void OnWeaponSelect( WeaponTile weaponTile ) {
    Debug.Log(weaponTile);
  }

  public void OnWeaponDeselect( WeaponTile weaponTile ) {
    Debug.Log("-" + weaponTile);
  }

}
