using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEvents : MonoBehaviour {

  public void NewGame() {
    GameDataController.Current.NewGame();
  }

  public void Continue() {
    Debug.Log("You pressed Continue");
  }

}
