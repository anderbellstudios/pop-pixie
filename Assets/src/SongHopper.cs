using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongHopper : MonoBehaviour {
  public Song Song;

  public bool HopOnStart = true;
  public bool StopOnDestroy = false;
  public bool PlayNext = false;

  public void Hop() {
    if (PlayNext) {
      MusicController.Current.PlayNext(Song);
    } else {
      MusicController.Current.Play(Song);
    }
  }

  public void Stop() {
    MusicController.Current.Stop();
  }

  void Start() {
    if (HopOnStart)
      Hop();
  }

  void OnDestroy() {
    if (StopOnDestroy)
      Stop();
  }
}
