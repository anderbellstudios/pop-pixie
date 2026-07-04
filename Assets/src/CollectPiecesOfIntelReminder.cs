using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectPiecesOfIntelReminder : MonoBehaviour {
  public bool ActivateOnStart;

  private List<PieceOfIntelSprite> PiecesOfIntel;
  private bool Active = false;

  void Start() {
    if (ActivateOnStart) {
      Activate();
    }

    PiecesOfIntel = FindObjectsOfType<PieceOfIntelSprite>().ToList();
  }

  public void CheckForIntel() {
    Debug.Log("check for intel is happening");
    int uncollected = PiecesOfIntel.Count(p => !p.Collected);
    if (uncollected != 0) {
      string verb = uncollected == 1 ? "is" : "are";
      string noun = uncollected == 1 ? "Piece of Intel" : "Pieces of Intel";
      string hintText =
        $"There {verb} <color=#ffff00>{uncollected}</color> undiscovered <color=#ffff00>{noun}</color> in this area";

      NotificationPrompt.Current.SetNewObjective(hintText);
    }
  }

  public void Activate() {
    Active = true;
  }

  public void Update() {
    string getIntelHintText = GetHintText();
    if (!string.IsNullOrEmpty(getIntelHintText)) {
      //Debug.Log("set objective to intel hint text");
      NotificationPrompt.Current.SetNewObjective(getIntelHintText);
    }
  }

  private string GetHintText() {
    if (!Active) {
      //Debug.Log("not active, returning null");
      return null;
    }

    int uncollected = PiecesOfIntel.Count(p => !p.Collected);

    if (uncollected == 0) {
      Debug.Log("uncollected=0, returning null");
      return null;
    }

    string verb = uncollected == 1 ? "is" : "are";
    string noun = uncollected == 1 ? "Piece of Intel" : "Pieces of Intel";

    return $"There {verb} <color=#ffff00>{uncollected}</color> undiscovered <color=#ffff00>{noun}</color> in this area";
  }
}
