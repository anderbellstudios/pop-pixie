using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaySound : MonoBehaviour {
  public bool PlayOnStart = false;
  public string DefaultProgrammerInstrumentKey = "";
  public FMODUnity.StudioEventEmitter EventEmitter;
  public bool Pausable = false;
  public UnityEvent OnPlay;

  public FMOD.Studio.EventInstance EventInstance => EventEmitter.EventInstance;

  private ProgrammerInstrumentUtils ProgrammerInstrumentUtils;
  private bool OverridePausable = false;

  void Start() {
    PreloadProgrammerSounds.PreloadSound(DefaultProgrammerInstrumentKey);

    ProgrammerInstrumentUtils = new ProgrammerInstrumentUtils();

    if (PlayOnStart) {
      Play();
    }

    if (Pausable) {
      StateManager.AddListener(() => {
        EventInstance.setPaused(
          !OverridePausable &&
          StateManager.Enabled(StateFeatures.PauseSounds)
        );
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

    OnPlay.Invoke();
  }

  public void Play(string programmerInstrumentKey, bool doNotPause) {
    OverridePausable = doNotPause;
    Play(programmerInstrumentKey);
  }

  public void Stop() {
    EventEmitter.Stop();
  }
}
