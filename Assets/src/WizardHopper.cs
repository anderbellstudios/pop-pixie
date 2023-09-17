using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardHopper : MonoBehaviour {
  public string Passphrase = "wizard";

  public List<char> Letters;

  void Awake() {
    Letters = new List<char>(
      Passphrase.ToCharArray()
    );
  }

  void Update() {
    if (Letters.Count == 0)
      return;

    if (Input.GetKeyDown(Letters[0].ToString())) {
      Letters.RemoveAt(0);
    }

    // Only on the first time
    if (Letters.Count == 0) {
      SceneEvents.Current.ChangeScene("Wizard Mode", true);
    }
  }
}
