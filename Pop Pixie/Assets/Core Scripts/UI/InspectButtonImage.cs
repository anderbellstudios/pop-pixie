using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectButtonImage : MonoBehaviour {

  public Image Image;
  public TMP_Text Label;

  void Update() {
    bool enabled = AInspectable.ShowButtonPrompt();
    Image.enabled = enabled;
    Label.enabled = enabled;
  }

}
