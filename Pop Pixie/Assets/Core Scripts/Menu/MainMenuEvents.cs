using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEvents : GenericMenuEvents {

  public void NewGame () {
    FadeOut(_NewGame);
  }

  public void Continue () {
    FadeOut(_Continue);
  }

  void _NewGame () {
    GameDataController.Current.NewGame();
  }

  void _Continue () {
    GameDataController.Current.Load();
  }

}
