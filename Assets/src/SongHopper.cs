using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongHopper : MonoBehaviour {

  public Song Song;

  public bool HopOnStart = true;
  public bool StopOnDestroy = false;

  void Start() {
    if ( HopOnStart ) Hop();
  }

  public void Hop() {
    SongStack.Current.Play( Song );
  }

  void OnDestroy() {
    if ( StopOnDestroy ) Stop();
  }

  public void Stop() {
    SongStack.Current.Stop();
  }

}
