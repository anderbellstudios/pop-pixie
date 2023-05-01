using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuffleSounds : MonoBehaviour {
  public AudioLowPassFilter AudioLowPassFilter;

  void Start() {
    StateManager.AddListener(() => {
      AudioLowPassFilter.enabled = StateManager.Enabled(StateFeatures.MuffleSounds);
    });
  }
}
