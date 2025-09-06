using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuEvents : AMenu {
  protected override void LocalClose() {
    StateManager.RemoveState(State.Paused);
    SceneManager.UnloadSceneAsync("Pause Menu");
  }
}
