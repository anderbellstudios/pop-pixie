using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCaptionLine : MonoBehaviour {
  public bool EnqueueIfBusy = true;
  public CaptionLine CaptionLine;

  void Start() {
    PreloadProgrammerSounds.PreloadCaptionLine(CaptionLine);
  }

  public void Perform() {
    CaptionLineManager.Current.Play(CaptionLine, enqueueIfBusy: EnqueueIfBusy);
  }
}
