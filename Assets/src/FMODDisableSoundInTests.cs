using System;
using UnityEngine;

[CreateAssetMenu(menuName = "FMOD Disable Sound In Tests")]
public class FMODDisableSoundInTests : FMODUnity.PlatformCallbackHandler {
  public static bool TestMode = false;

  public override void PreInitialize(FMOD.Studio.System studioSystem, Action<FMOD.RESULT, string> reportResult) {
    FMOD.System coreSystem;
    FMOD.RESULT result = studioSystem.getCoreSystem(out coreSystem);
    reportResult(result, "studioSystem.getCoreSystem");

    // FMOD doesn't work in Ubuntu Docker container due to lack of PulseAudio
    if (TestMode) {
      coreSystem.setOutput(FMOD.OUTPUTTYPE.NOSOUND);
    }
  }
}
