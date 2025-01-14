using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutput {
  public string Name;
  public Guid Guid;

  /**
   * Potentially volatile. Do not use to identify a driver longer than
   * instantaneously.
   */
  protected int Index;

  public static List<AudioOutput> GetAll() {
    FMOD.System system;
    FMODUnity.RuntimeManager.StudioSystem.getCoreSystem(out system);

    int count;
    system.getNumDrivers(out count);

    List<AudioOutput> audioOutputs = new();

    for (int i = 0; i < count; i++) {
      string name;
      Guid guid;

      system.getDriverInfo(
        i,
        out name,
        256,
        out guid,
        out _, // systemrate
        out _, // speakermode
        out _ // speakermodechannels
      );

      audioOutputs.Add(new AudioOutput {
        Name = name,
        Guid = guid,
        Index = i
      });
    }

    return audioOutputs;
  }

  public static AudioOutput GetDefault() => GetAll()[0];

  public static AudioOutput Find(Guid guid)
    => GetAll().Find(audioOutput => audioOutput.Guid == guid);

  public static void Set(Guid guid) {
    AudioOutput audioOutput = Find(guid);

    if (audioOutput == null) {
#if UNITY_EDITOR
      Debug.LogError($"Audio output not available: {guid}. Setting to default.");
#endif
      SetToDefault();
      return;
    }

    Set(audioOutput.Index);
    OptionsData.AudioOutput = guid;
  }

  public static void SetToDefault() {
    Set(0);
    OptionsData.AudioOutput = null;
  }

  protected static void Set(int index) {
    FMOD.System system;
    FMODUnity.RuntimeManager.StudioSystem.getCoreSystem(out system);
    system.setDriver(index);
  }

  public static void Initialise() {
    Guid? currentGuid = OptionsData.AudioOutput;

#if UNITY_EDITOR
    Debug.Log($"Setting audio output: {currentGuid?.ToString() ?? "default"}");
#endif

    if (currentGuid.HasValue) {
      Set(currentGuid.Value);
    }
  }
}
