using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHopper : MonoBehaviour {
  public bool AboutToSave = false;

  void Start() {
    // Using Update so that everything had a chance to Start
    GDCall.UnlessLoad( HopSaveYourGame );
  }

  public void HopSaveYourGame() {
    AboutToSave = true;
  }

  void Update() {
    if ( AboutToSave ) {
      AboutToSave = false;

      GameData.Save();
      GameData.Write();
    }
  }

}
