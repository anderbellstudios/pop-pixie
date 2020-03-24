using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSongOnDeath : MonoBehaviour, IHitPointEvents {

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
  }

  public void BecameZero (HitPoints hp) {
    AudioMixer.Current.FadeOut(5);
  }

}
