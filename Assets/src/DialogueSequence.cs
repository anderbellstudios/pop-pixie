using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSequence {
  public DialogueMusicFadeBehaviour DialogueMusicFadeBehaviour = new DialogueMusicFadeBehaviour(
    DialogueMusicFadeBehaviour.BehaviourType.FadeDown,
    DialogueMusicFadeBehaviour.BehaviourType.FadeUp
  );

  public List<DialoguePage> Pages;

  public int PageCount
    => Pages.Count;

  public DialoguePage GetPage(int index)
    => Pages[index];
}
