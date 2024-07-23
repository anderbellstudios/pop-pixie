using System;
using System.Runtime.InteropServices;

public class ProgrammerInstrumentUtils {
  private FMOD.Studio.EVENT_CALLBACK EventCallback;

  public ProgrammerInstrumentUtils() {
    EventCallback = new FMOD.Studio.EVENT_CALLBACK(OnEventCallback);
  }

  public void LinkSound(
    FMOD.Studio.EventInstance instance,
    string key
  ) {
    GCHandle keyHandle = GCHandle.Alloc(key);
    instance.setUserData(GCHandle.ToIntPtr(keyHandle));
    instance.setCallback(EventCallback);
  }

  [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
  public static FMOD.RESULT OnEventCallback(
    FMOD.Studio.EVENT_CALLBACK_TYPE type,
    IntPtr instancePointer,
    IntPtr parameterPointer
  ) {
    FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePointer);

    IntPtr keyPointer;
    instance.getUserData(out keyPointer);
    GCHandle keyHandle = GCHandle.FromIntPtr(keyPointer);
    String key = keyHandle.Target as String;

    switch (type) {
      case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
        return OnCreateProgrammerSound(instance, key, parameterPointer);

      case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
        return OnDestroyProgrammerSound(parameterPointer);

      case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
        keyHandle.Free();
        break;

      default:
        break;
    }

    return FMOD.RESULT.OK;
  }

  static FMOD.RESULT OnCreateProgrammerSound(
    FMOD.Studio.EventInstance instance,
    string key,
    IntPtr parameterPointer
  ) {
    FMOD.MODE soundMode =
      FMOD.MODE.LOOP_NORMAL |
      FMOD.MODE.CREATECOMPRESSEDSAMPLE |
      FMOD.MODE.NONBLOCKING;

    FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES parameter = GetParameter(parameterPointer);

    FMOD.Studio.SOUND_INFO soundInfo;
    FMOD.RESULT keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out soundInfo);
    if (keyResult != FMOD.RESULT.OK)
      return keyResult;

    FMOD.Sound sound;
    FMOD.RESULT soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(
      soundInfo.name_or_data,
      soundMode | soundInfo.mode,
      ref soundInfo.exinfo,
      out sound
    );

    if (soundResult != FMOD.RESULT.OK)
      return soundResult;

    parameter.sound = sound.handle;
    parameter.subsoundIndex = soundInfo.subsoundindex;
    Marshal.StructureToPtr(parameter, parameterPointer, false);

    return FMOD.RESULT.OK;
  }

  static FMOD.RESULT OnDestroyProgrammerSound(IntPtr parameterPointer) {
    FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES parameter = GetParameter(parameterPointer);
    FMOD.Sound sound = new FMOD.Sound(parameter.sound);
    sound.release();
    return FMOD.RESULT.OK;
  }

  static FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES GetParameter(IntPtr parameterPointer)
    => (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(
      parameterPointer,
      typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)
    );
}
