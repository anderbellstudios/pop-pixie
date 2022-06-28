using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneLinePhase : APhase {
  [TextArea] public string Text;
  public float Duration, FadeOutDelay, FadeOutDuration;

  public override void LocalBegin() {
    CutsceneTextManager.Current.Write(Text, Duration, () => {
      Invoke("FadeOut", FadeOutDelay - FadeOutDuration);
    });
  }

  void FadeOut() {
    CutsceneTextManager.Current.FadeOut(FadeOutDuration, PhaseFinished);
  }
}
