using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGameStarted : MonoBehaviour {

  public ScreenFade Fader;

  void Start () {
    Fader.Fade("from black", 3.0f);
  }
}
