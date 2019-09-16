using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiscoveredItemsMenuEvents : MonoBehaviour {

  public static PauseMenuEvents ParentMenu; // Must be set by object that calls LoadScene

  void Update() {
    if ( WrappedInput.GetButtonDown("Cancel") ) {
      ResumeParent();
    }
  }

  public void ResumeParent() {
    SceneManager.UnloadSceneAsync("Discovered Items Menu");
    ParentMenu.Focus();
  }

}
