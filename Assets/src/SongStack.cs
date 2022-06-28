using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongStack : MonoBehaviour {

  public SongController SongController;

  Stack<Song> Songs = new Stack<Song>();

  public bool SingletonInstance = true;
  public static SongStack Current;

  void Awake() {
    if (SingletonInstance)
      Current = this;
  }

  public void Play( Song song ) {
    Songs.Push(song);
    UpdateController();
  }

  public void Stop() {
    Songs.Pop();
    UpdateController();
  }

  void UpdateController() {
    SongController.Play( CurrentSong() );
  }

  Song CurrentSong() {
    if ( Songs.Count == 0 ) {
      return null;
    } else {
      return Songs.Peek();
    }
  }

}
