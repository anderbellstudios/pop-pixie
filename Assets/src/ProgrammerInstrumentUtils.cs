using System;
using System.Runtime.InteropServices;

public class ProgrammerInstrumentUtils {
  private FMOD.Studio.EVENT_CALLBACK EventCallback;

  public ProgrammerInstrumentUtils() {
    EventCallback = new FMOD.Studio.EVENT_CALLBACK(OnEventCallback);
  }

  public void LinkSound(FMOD.Studio.EventInstance instance, string key) {
    GCHandle soundDataHandle = PreloadProgrammerSounds.SoundDataForKey(key);
    instance.setUserData(GCHandle.ToIntPtr(soundDataHandle));
    instance.setCallback(EventCallback);
  }

  [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
  public static FMOD.RESULT OnEventCallback(
    FMOD.Studio.EVENT_CALLBACK_TYPE type,
    IntPtr instancePointer,
    IntPtr parameterPointer
  ) {
    if (type != FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND) {
      return FMOD.RESULT.OK;
    }

    FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePointer);

    IntPtr soundDataPointer;
    instance.getUserData(out soundDataPointer);
    GCHandle soundDataHandle = GCHandle.FromIntPtr(soundDataPointer);
    PreloadProgrammerSounds.SoundData soundData = (PreloadProgrammerSounds.SoundData)
      soundDataHandle.Target;

    FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)
      Marshal.PtrToStructure(parameterPointer, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

    parameter.sound = soundData.Sound.handle;
    parameter.subsoundIndex = soundData.SoundInfo.subsoundindex;
    Marshal.StructureToPtr(parameter, parameterPointer, false);

    return FMOD.RESULT.OK;
  }
}
