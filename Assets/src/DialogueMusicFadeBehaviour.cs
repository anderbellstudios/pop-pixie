using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMusicFadeBehaviour {
  public enum BehaviourType { FadeUp, FadeDown, Ignore }

  public BehaviourType EnterBehaviour, ExitBehaviour;

  public DialogueMusicFadeBehaviour(BehaviourType enterBehaviour, BehaviourType exitBehaviour) {
    EnterBehaviour = enterBehaviour;
    ExitBehaviour = exitBehaviour;
  }

  public void ApplyEnterBehaviour() => ApplyBehaviour(EnterBehaviour);
  public void ApplyExitBehaviour() => ApplyBehaviour(ExitBehaviour);

  private void ApplyBehaviour(BehaviourType behaviour) {
    switch (behaviour) {
      case BehaviourType.FadeUp:
        AudioManager.Current.SetDuringDialogue(false);
        break;

      case BehaviourType.FadeDown:
        AudioManager.Current.SetDuringDialogue(true);
        break;

      case BehaviourType.Ignore:
        break;
    }
  }
}
