using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleted : MonoBehaviour, IDialogueSequenceEventHandler, ISerializableComponent {

  public string[] SerializableFields { get; } = { "KeycardDialoguePlayed" };

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
    if (StateManager.Isnt(State.Playing))
      return;
    CancelInvoke();
    KeycardDialoguePlayed = true;
    Dialogue.Play("Dialogue/Keycard", this);
  }

  public void SequenceFinished() {}
}
