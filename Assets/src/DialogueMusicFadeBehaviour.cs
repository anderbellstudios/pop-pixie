using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMusicFadeBehaviour {
  public enum BehaviourType { SetHigh, SetLow, Ignore }

  public BehaviourType EnterBehaviour, ExitBehaviour;

  public DialogueMusicFadeBehaviour(BehaviourType enterBehaviour, BehaviourType exitBehaviour) {
    EnterBehaviour = enterBehaviour;
    ExitBehaviour = exitBehaviour;
  }

  public void ApplyEnterBehaviour() => ApplyBehaviour(EnterBehaviour);
  public void ApplyExitBehaviour() => ApplyBehaviour(ExitBehaviour);

  private void ApplyBehaviour(BehaviourType behaviour) {
    switch (behaviour) {
      case BehaviourType.SetHigh:
        AudioFadeOut.Current.SetDialogueMusicFade(false);
        break;

      case BehaviourType.SetLow:
        AudioFadeOut.Current.SetDialogueMusicFade(true);
        break;

      case BehaviourType.Ignore:
        break;
    }
  }
}
