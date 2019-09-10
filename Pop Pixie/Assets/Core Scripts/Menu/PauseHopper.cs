using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHopper : MonoBehaviour {

  void Update() {
    if ( StateManager.Isnt( State.Playing ) )
      return;

    if ( WrappedInput.GetButtonDown("Pause") ) {
      GameData.Save();
      GameData.WriteAutoSave();

      StateManager.SetState( State.Paused );
      SceneManager.LoadScene( "Pause Menu", LoadSceneMode.Additive );
    }
      
  }

}
