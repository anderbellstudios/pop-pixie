using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSequence {
  public DialoguePage[] Pages;

  public static DialogueSequence ParseJSON (string json) {
    return JsonUtility.FromJson<DialogueSequence>(json);
  }
}
