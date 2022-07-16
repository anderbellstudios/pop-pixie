using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPause : MonoBehaviour {
  void Start() {
    StateManager.AddListener(() => {
      AudioListener.pause = StateManager.Enabled(StateFeatures.PauseAudio);
    });
  }
}
