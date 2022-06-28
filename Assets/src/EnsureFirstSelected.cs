using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnsureFirstSelected : MonoBehaviour {

  public Button Button;

  void Start() {
    Button.Select();
    Button.OnSelect(null);
  }

}
