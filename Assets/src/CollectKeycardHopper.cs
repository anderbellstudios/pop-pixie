using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectKeycardHopper : MonoBehaviour {
  public DialogueHopper GotKeycardDialogue;

  public void Hop() {
    if (!LevelObjectives.GotKeycard) {
      LevelObjectives.GotKeycard = true;
      GotKeycardDialogue.Hop();
    }
  }
}
