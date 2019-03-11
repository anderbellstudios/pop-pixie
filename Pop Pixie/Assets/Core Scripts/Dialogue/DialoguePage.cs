using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePage {
  public string FaceId;
  public string VoiceLinePath;
  public string Text;

  public Sprite Face () {
    return Resources.Load<Sprite>("Faces/" + FaceId);
  }

  public bool HasVoiceLine () {
    return VoiceLinePath != null;
  }

  public AudioClip VoiceLine () {
    return Resources.Load<AudioClip>("Voice Lines/" + VoiceLinePath);
  }
}
