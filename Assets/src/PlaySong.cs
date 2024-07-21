using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour {
  public static PlaySong Current;
  private static FMOD.GUID? CurrentSongGuid = null;
  private static FMOD.Studio.EventInstance? CurrentSongEventInstance = null;

  public bool PlayOnStart = true;
  public FMODUnity.StudioEventEmitter EventEmitter;

  private FMOD.GUID EventGuid => EventEmitter.EventReference.Guid;
  private FMOD.Studio.EventInstance EventInstance => EventEmitter.EventInstance;

  void Start() {
    if (PlayOnStart) {
      Play();
    }
  }

  public void Play() {
    // If the song is already playing, nothing to do
    if (CurrentSongGuid == EventGuid) return;

    if (CurrentSongGuid != null) {
      Debug.LogWarning("Another song is already playing. Stopping it abruptly."); 
      CurrentSongEventInstance?.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
      CurrentSongEventInstance?.release();
    }

    EventEmitter.Play();

    Current = this;
    CurrentSongGuid = EventGuid;
    CurrentSongEventInstance = EventInstance;
  }

  public void Stop() {
    EventEmitter.Stop();
    Current = null;
    CurrentSongGuid = null;
    CurrentSongEventInstance = null;
  }
}
