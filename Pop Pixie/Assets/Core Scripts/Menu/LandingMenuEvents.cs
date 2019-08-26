using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingMenuEvents : GenericMenuEvents {

  public void Begin() {
    FadeOut(MainMenu);
  }

  void MainMenu () {
    SceneManager.LoadScene("Main Menu");
  }

}
