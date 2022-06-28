using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InspectButtonHint : MonoBehaviour {
  public TMP_Text Label;

  void Update() {
    Label.enabled = AInspectable.ShowButtonPrompt();
  }
}
