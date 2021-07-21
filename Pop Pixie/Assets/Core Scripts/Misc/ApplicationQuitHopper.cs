using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitHopper : MonoBehaviour {
  public void Hop() {
    WrappedApplication.Quit();
  }
}
