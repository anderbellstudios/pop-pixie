using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleted : MonoBehaviour, IDialogueSequenceEventHandler {

  public ElevatorBarrier ElevatorBarrier;
  public DialogueManager Dialogue;
  public float Delay = 1.5f;

  public void Run() {
    Invoke("ShowMessage", Delay);
  }

  void ShowMessage() {
    Dialogue.Play("Dialogue/Keycard", this);
  }

  public void SequenceFinished() {
    ElevatorBarrier.Remove();
  }
}
