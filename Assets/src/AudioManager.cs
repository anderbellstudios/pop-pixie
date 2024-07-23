using System;
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
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(
      "User music volume",
      ConvertVolumeToParam((float)OptionsData.MusicVolume)
    );

    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(
      "User SFX volume",
      ConvertVolumeToParam((float)OptionsData.SoundsVolume)
    );

    FMODUnity.RuntimeManager.StudioSystem.setParameterByName(
      "User voice volume",
      ConvertVolumeToParam((float)OptionsData.VoiceVolume)
    );
  }

  public void SetDuringDialogue(bool duringDialogue) {
    if (duringDialogue) {
      DuringDialogueSnapshot.Play();
    } else {
      DuringDialogueSnapshot.Stop();
    }
  }

  public static float ConvertVolumeToParam(float volume)
    => Mathf.Clamp(ConvertDBToParam(ConvertVolumeToDB(volume)), 0f, 1f);

  private static float ConvertVolumeToDB(float volume)
    => volume > 0f ? 20f * Mathf.Log(volume, 10f) : -80f;

  /**
   * When controlling volume using a parameter in FMOD, there's an arbitrary
   * mapping between parameter values and decibel values. The following
   * polynomial approximates this mapping.
   * https://qa.fmod.com/t/feature-request-logarithmic-parameter-types-for-gain-and-freq-control-or-linear-controls/14947/8
   */
  private static float ConvertDBToParam(double dB)
    => (float)((-2.439E-08 * Math.Pow(dB, 4)) +
        (-2.851E-06 * Math.Pow(dB, 3)) +
        (9.985E-05 * Math.Pow(dB, 2)) +
        (0.0263 * Math.Pow(dB, 1)) +
        1.0054);
}
