﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenuEvents : AMenu {
  public AMenu DiscoveredItemsMenu, OptionsMenu;

  public override void LocalClose() {
    StateManager.SetState( State.Playing );
    SceneManager.UnloadSceneAsync("Pause Menu");
  }

  public void DiscoveredItems() {
    OpenNestedMenu(DiscoveredItemsMenu);
  }

  public void Options() {
    OpenNestedMenu(OptionsMenu);
  }
}
