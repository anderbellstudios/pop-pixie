using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePage {
  public string Speaker;
  public Sprite Face;
  public AudioClip AudioClip;
  public float RelativeSpeed = 1f;
  [TextArea] public string Text;

  public bool HasAudioClip()
    => AudioClip != null;
}
