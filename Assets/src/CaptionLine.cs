using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CaptionLine {
  public string Text;
  public AudioClip AudioClip;
  public float Duration;

  public DialogueMusicFadeBehaviour DialogueMusicFadeBehaviour = new DialogueMusicFadeBehaviour(
    DialogueMusicFadeBehaviour.BehaviourType.SetLow,
    DialogueMusicFadeBehaviour.BehaviourType.SetHigh
  );
}
