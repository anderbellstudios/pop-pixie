using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePage {
  public string Speaker;
  public Sprite Face;
  public string VoiceLineKey;
  public float AutoAdvanceDelay;

  [TextArea]
  public string Text;

  public bool HasAudioClip() => VoiceLineKey.Length > 0;

  public bool ShouldAutoAdvance() => AutoAdvanceDelay > 0;

  private int _Hash = 0;

  public int Hash {
    get {
      // https://stackoverflow.com/a/5155015
      if (_Hash == 0) {
        _Hash = 23;

        foreach (char c in Text)
          unchecked {
            _Hash = _Hash * 31 + c;
          }
      }

      return _Hash;
    }
  }
}
