using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueHopper : MonoBehaviour {
  [SerializeField] public DialogueSequence DialogueSequence;

  [SerializeField] public UnityEvent OnFinish;

  public void Hop() {
    DialogueManager.Current.Play(DialogueSequence, () => {
      OnFinish.Invoke();
    });
  }
}
