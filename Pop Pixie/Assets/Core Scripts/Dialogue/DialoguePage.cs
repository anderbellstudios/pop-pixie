using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePage {
  public Sprite Face;
  public AudioClip AudioClip;

  [TextArea]
  public string Text;

  public bool HasAudioClip()
    => AudioClip != null;
}
