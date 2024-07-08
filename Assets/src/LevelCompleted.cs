using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleted : MonoBehaviour {
  public ElevatorBarrier ElevatorBarrier;
  public DialogueManager Dialogue;
  public float Delay = 1.5f;

  public bool KeycardDialoguePlayed = false;

  public void Run() {
    ElevatorBarrier.Remove();

    if (!KeycardDialoguePlayed)
      InvokeRepeating("TryShowMessage", Delay, Delay);
  }

  void TryShowMessage() {
    if (!StateManager.Playing)
      return;
    CancelInvoke();
    KeycardDialoguePlayed = true;
    // Dialogue.Play("Dialogue/Keycard", this);
  }
}
