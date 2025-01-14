using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rewired;

public class AudioSettingsMenuEvents : AMenu {
  public StepperInput MusicVolumeStepper, SoundsVolumeStepper, VoiceVolumeStepper;
  public SelectMenu SelectAudioOutput;

  public override void LocalStart() {
    MusicVolumeStepper.Options = SoundsVolumeStepper.Options = VoiceVolumeStepper.Options =
      Enumerable.Range(0, 11).Select(n => String.Format("{0}%", n * 10)).ToList();

    MusicVolumeStepper.Value = (int)(OptionsData.MusicVolume * 10);
    MusicVolumeStepper.UpdateLabel();

    SoundsVolumeStepper.Value = (int)(OptionsData.SoundsVolume * 10);
    SoundsVolumeStepper.UpdateLabel();

    VoiceVolumeStepper.Value = (int)(OptionsData.VoiceVolume * 10);
    VoiceVolumeStepper.UpdateLabel();

    MusicVolumeStepper.OnChange.AddListener(MusicVolumeChanged);
    SoundsVolumeStepper.OnChange.AddListener(SoundsVolumeChanged);
    VoiceVolumeStepper.OnChange.AddListener(VoiceVolumeChanged);
  }

  public void AudioOutput() {
    List<SelectMenuOption> options = new();
    Guid? currentGuid = OptionsData.AudioOutput;

    options.Add(new SelectMenuOption {
      Name = "System output",
      OnSelect = global::AudioOutput.SetToDefault
    });

    int selectedIndex = 0;
    int i = 1;

    global::AudioOutput.GetAll().ForEach(audioOutput => {
      options.Add(new SelectMenuOption {
        Name = audioOutput.Name,
        OnSelect = () => global::AudioOutput.Set(audioOutput.Guid)
      });

      if (audioOutput.Guid == currentGuid) {
        selectedIndex = i;
      }

      i++;
    });

    SelectAudioOutput.SetOptions(options, selectedIndex);
    OpenNestedMenu(SelectAudioOutput);
  }

  public void MusicVolumeChanged(int index, string label) {
    OptionsData.MusicVolume = ((decimal)index) * 0.1M;
  }

  public void SoundsVolumeChanged(int index, string label) {
    OptionsData.SoundsVolume = ((decimal)index) * 0.1M;
  }

  public void VoiceVolumeChanged(int index, string label) {
    OptionsData.VoiceVolume = ((decimal)index) * 0.1M;
  }
}
