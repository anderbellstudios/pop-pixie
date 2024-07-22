using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
  void Awake() {
    UpdateVolumes();
    ConfigData.Current.OnChange.AddListener(UpdateVolumes);
  }

  void OnDestroy() {
    ConfigData.Current.OnChange.RemoveListener(UpdateVolumes);
  }

  void UpdateVolumes() {
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("User music volume", (float) OptionsData.MusicVolume);
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("User SFX volume", (float) OptionsData.SoundsVolume);
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("User voice volume", (float) OptionsData.VoiceVolume);
  }
}
