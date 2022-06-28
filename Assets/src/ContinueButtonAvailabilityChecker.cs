using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonAvailabilityChecker : MonoBehaviour {

  public GameObject Button;

  void Awake() {
    if ( !SaveGame.Exists() )
      Destroy(Button);
  }

}
