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

}
