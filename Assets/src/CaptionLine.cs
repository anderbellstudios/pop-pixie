using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CaptionLine {
  [TextArea] public string Text;
  public string VoiceLineKey;
  public float Duration;
  public bool IgnorePause = false;

  public DialogueMusicFadeBehaviour DialogueMusicFadeBehaviour = new DialogueMusicFadeBehaviour(
    DialogueMusicFadeBehaviour.BehaviourType.FadeDown,
    DialogueMusicFadeBehaviour.BehaviourType.FadeUp
  );

  public bool HasAudioClip() => VoiceLineKey.Length > 0;
}
