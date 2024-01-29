using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuEvents : AMenu {
  public override void LocalClose() {
    StateManager.RemoveState(State.Paused);
    SceneManager.UnloadSceneAsync("Pause Menu");
  }
}
