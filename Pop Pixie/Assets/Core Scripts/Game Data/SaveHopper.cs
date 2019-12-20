using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHopper : MonoBehaviour {
  public bool AboutToSave = false;
  public bool AboutToInvokeAutoSave = false;

  void Start() {
    // Using Update so that everything had a chance to Start
    GDCall.UnlessLoad( HopSaveYourGame );
    AboutToInvokeAutoSave = true;
  }

  public void HopSaveYourGame() {
    AboutToSave = true;
  }

  void Update() {
    if ( AboutToSave ) {
      AboutToSave = false;

      GameData.Save();
      GameData.WriteSave();
    }

    if ( AboutToInvokeAutoSave ) {
      AboutToInvokeAutoSave = false;
      InvokeRepeating( "AutoSave", 5.0f, 5.0f );
    }
  }

  void AutoSave() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    Debug.Log("Hop!");
    SongController.Current.RecordPlaybackTime();
    GameData.Save();
    GameData.WriteAutoSave();
  }

}
