using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueMusicFadeBehaviour {
  public enum BehaviourType { FadeUp, FadeDown, Ignore, SetHigh, SetLow }

  public static float FadeDuration = 0.5f;
  public static float FadedVolume = 0.25f;

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
        MusicController.Current.SetFadeLevel(1f);
        break;

      case BehaviourType.SetLow:
        MusicController.Current.SetFadeLevel(FadedVolume);
        break;

      case BehaviourType.FadeUp:
        MusicController.Current.FadeTo(1f, FadeDuration);
        break;

      case BehaviourType.FadeDown:
        MusicController.Current.FadeTo(FadedVolume, FadeDuration);
        break;

      case BehaviourType.Ignore:
        break;
    }
  }
}
