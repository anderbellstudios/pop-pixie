using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLinkHopper : MonoBehaviour {
  public string URL;

  public void Hop() {
    Application.OpenURL(URL);
  }
}
