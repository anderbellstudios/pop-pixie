using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHopper : MonoBehaviour {
  public string SceneName;

  public void Hop() {
    SceneManager.LoadScene(SceneName);
  }
}
