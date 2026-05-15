using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectPiecesOfIntelReminder : MonoBehaviour {
  public bool ActivateOnStart;

  private List<PieceOfIntelSprite> PiecesOfIntel;
  private bool Active = false;

  void Start() {
    if (ActivateOnStart)
      Activate();

    PiecesOfIntel = FindObjectsOfType<PieceOfIntelSprite>().ToList();

    NotificationPrompt.Current.RegisterSource(NotificationPrompt.Priority.CollectIntel, HintText);
  }

  public void Activate() {
    Active = true;
  }

  string HintText() {
    if (!Active)
      return null;

    int uncollected = PiecesOfIntel.Count(p => !p.Collected);

    if (uncollected == 0)
      return null;

    string verb = uncollected == 1 ? "is" : "are";
    string noun = uncollected == 1 ? "Piece of Intel" : "Pieces of Intel";

    return $"There {verb} <color=#ffff00>{uncollected}</color> undiscovered <color=#ffff00>{noun}</color> in this area";
  }
}
