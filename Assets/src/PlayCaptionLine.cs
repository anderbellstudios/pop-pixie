using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCaptionLine : MonoBehaviour {
  public CaptionLine CaptionLine;

  public void Perform() {
    CaptionLineManager.Current.Play(CaptionLine);
  }
}
