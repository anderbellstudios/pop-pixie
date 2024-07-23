using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
  public bool PlayOnStart = false;
  public FMODUnity.StudioEventEmitter EventEmitter;
  public bool Pausable = false; // TODO

  private ProgrammerInstrumentUtils ProgrammerInstrumentUtils;

  void Start() {
    ProgrammerInstrumentUtils = new ProgrammerInstrumentUtils();

    if (PlayOnStart) {
      Play();
    }
  }

  public void Play(string programmerInstrumentKey = "") {
    EventEmitter.Play();

    if (programmerInstrumentKey != "") {
      ProgrammerInstrumentUtils.LinkSound(
        EventEmitter.EventInstance,
        programmerInstrumentKey
      );
    }
  }
}
