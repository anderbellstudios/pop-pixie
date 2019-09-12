using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectButtonImage : MonoBehaviour {

  public Image Image;

  void Update() {
    Image.enabled = AInspectable.ShowButtonPrompt();
  }

}
