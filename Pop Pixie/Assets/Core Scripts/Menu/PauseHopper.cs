using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHopper : MonoBehaviour {

  public bool SaveOnPause = true;
  public bool MinimalPauseMenu = false;

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( WrappedInput.GetButtonDown("Pause") ) {
      if (SaveOnPause) {
        SceneData.Save();
        SaveGame.WriteAutoSave();
      }

      PauseMenuData.MinimalPauseMenu = MinimalPauseMenu;

      StateManager.SetState( State.Paused );
      SceneManager.LoadScene( "Pause Menu", LoadSceneMode.Additive );
    }
      
  }

}
