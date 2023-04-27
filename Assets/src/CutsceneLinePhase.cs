using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneLinePhase : APhase {
  [TextArea] public string Text;
  public float TotalDuration, TypewriterDuration, FadeOutDuration;

  public override void LocalBegin() {
    CutsceneTextManager.Current.Write(
      Text,
      TotalDuration,
      TypewriterDuration,
      FadeOutDuration,
      PhaseFinished
    );
  }
}
