using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectKeycardHopper : MonoBehaviour {
  public DialogueHopper GotKeycardDialogue;

  public void Hop() {
    LevelObjectives.Current.GotKeycard = true;
    GotKeycardDialogue.Hop();
  }
}
