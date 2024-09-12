using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

// https://qa.fmod.com/t/loading-delay-exceeded-warning-when-using-programmer-instrument-and-audio-table/15737/10
public class PreloadProgrammerSounds : MonoBehaviour {
  public static PreloadProgrammerSounds Current;

  public struct SoundData {
    public Sound Sound;
    public SOUND_INFO SoundInfo;
  }

  private Dictionary<string, GCHandle> SoundDataByKey = new();

  public static void PreloadDialogue(DialogueSequence dialogueSequence) {
    dialogueSequence.Pages.ForEach(page => {
      PreloadSound(page.VoiceLineKey);
    });
  }

  public static void PreloadCaptionLine(CaptionLine captionLine) {
    PreloadSound(captionLine.VoiceLineKey);
  }

  public static void PreloadSound(string key) {
    if (key == null || key.Length == 0)
      return;
    if (Current.SoundDataByKey.ContainsKey(key))
      return;

    MODE soundMode =
      MODE.LOOP_NORMAL |
      MODE.CREATECOMPRESSEDSAMPLE |
      MODE.NONBLOCKING;

    SOUND_INFO soundInfo;
    RESULT keyResult = RuntimeManager.StudioSystem.getSoundInfo(key, out soundInfo);
    HandleResult(keyResult, "Failed to get sound info for key: " + key);

    Sound sound;
    RESULT soundResult = RuntimeManager.CoreSystem.createSound(
      soundInfo.name_or_data,
      soundMode | soundInfo.mode,
      ref soundInfo.exinfo,
      out sound
    );
    HandleResult(soundResult, "Failed to create sound for key: " + key);

    SoundData soundData = new SoundData() {
      Sound = sound,
      SoundInfo = soundInfo
    };

    GCHandle soundDataHandle = GCHandle.Alloc(soundData, GCHandleType.Pinned);
    Current.SoundDataByKey[key] = soundDataHandle;
  }

  public static GCHandle SoundDataForKey(string key) {
    if (!Current.SoundDataByKey.ContainsKey(key)) {
      throw new Exception("Tried to play programmer sound that hasn't been preloaded: " + key);
    }

    return Current.SoundDataByKey[key];
  }

  void Awake() {
    Current = this;
  }

  void OnDestroy() {
    foreach (GCHandle soundDataHandle in SoundDataByKey.Values) {
      SoundData soundData = (SoundData)soundDataHandle.Target;
      soundData.Sound.release();
      soundDataHandle.Free();
    }
  }

  static void HandleResult(RESULT result, string message) {
    if (result != RESULT.OK) {
      throw new Exception(message);
    }
  }
}
