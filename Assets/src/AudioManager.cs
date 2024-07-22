using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static AudioManager Current;

  public FMODUnity.StudioEventEmitter MuffleSnapshot, DuringDialogueSnapshot;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    UpdateVolumes();
    ConfigData.Current.OnChange.AddListener(UpdateVolumes);
  }

  void Start() {
    StateManager.AddListener(() => {
      if (StateManager.Enabled(StateFeatures.MuffleSounds)) {
        MuffleSnapshot.Play();
      } else {
        MuffleSnapshot.Stop();
      }
    });
  }

  void OnDestroy() {
    ConfigData.Current.OnChange.RemoveListener(UpdateVolumes);
  }

  void UpdateVolumes() {
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("User music volume", (float)OptionsData.MusicVolume);
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("User SFX volume", (float)OptionsData.SoundsVolume);
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("User voice volume", (float)OptionsData.VoiceVolume);
  }

  public void SetDuringDialogue(bool duringDialogue) {
    if (duringDialogue) {
      DuringDialogueSnapshot.Play();
    } else {
      DuringDialogueSnapshot.Stop();
    }
  }
}
