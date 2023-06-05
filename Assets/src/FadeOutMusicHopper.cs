using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutMusicHopper : MonoBehaviour {
  public float Duration;

  public void Hop() {
    MusicController.Current.FadeOut(Duration);
  }
}
