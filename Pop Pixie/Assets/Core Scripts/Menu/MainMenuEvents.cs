﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : GenericMenuEvents {

  public void NewGame () {
    FadeOut(_NewGame);
  }

  public void Continue () {
    FadeOut(_Continue);
  }

  void _NewGame () {
    SceneManager.LoadScene("Level1");
  }

  void _Continue () {
    GameData.ReadAutoSave();
    GameData.Load();
  }

}