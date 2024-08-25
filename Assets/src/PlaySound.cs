using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
  public bool PlayOnStart = false;
  public string DefaultProgrammerInstrumentKey = "";
  public FMODUnity.StudioEventEmitter EventEmitter;
  public bool PauseWhenNotPlaying = false;

  public FMOD.Studio.EventInstance EventInstance => EventEmitter.EventInstance;

  private ProgrammerInstrumentUtils ProgrammerInstrumentUtils;
  private bool OverridePauseWhenNotPlaying = false;

  void Start() {
    PreloadProgrammerSounds.PreloadSound(DefaultProgrammerInstrumentKey);

    ProgrammerInstrumentUtils = new ProgrammerInstrumentUtils();

    if (PlayOnStart) {
      Play();
    }

    if (PauseWhenNotPlaying) {
      StateManager.AddListener(() => {
        EventInstance.setPaused(!OverridePauseWhenNotPlaying && !StateManager.Playing);
      });
    }
  }

  public void Play(string programmerInstrumentKey = "") {
    EventEmitter.Play();

    if (programmerInstrumentKey == "") {
      programmerInstrumentKey = DefaultProgrammerInstrumentKey;
    }

    if (programmerInstrumentKey != "") {
      ProgrammerInstrumentUtils.LinkSound(
        EventEmitter.EventInstance,
        programmerInstrumentKey
      );
    }
  }

  public void Play(string programmerInstrumentKey, bool doNotPause) {
    OverridePauseWhenNotPlaying = doNotPause;
    Play(programmerInstrumentKey);
  }

  public void Stop() {
    EventEmitter.Stop();
  }
}
