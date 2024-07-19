using System;
using UnityEngine;

[CreateAssetMenu(menuName = "FMOD Disable Sound In Tests")]
public class FMODDisableSoundInTests : FMODUnity.PlatformCallbackHandler {
  public override void PreInitialize(FMOD.Studio.System studioSystem, Action<FMOD.RESULT, string> reportResult) {
    FMOD.System coreSystem;
    FMOD.RESULT result = studioSystem.getCoreSystem(out coreSystem);
    reportResult(result, "studioSystem.getCoreSystem");

    // TODO: Only do this when running tests
    coreSystem.setOutput(FMOD.OUTPUTTYPE.NOSOUND);
  }
}
