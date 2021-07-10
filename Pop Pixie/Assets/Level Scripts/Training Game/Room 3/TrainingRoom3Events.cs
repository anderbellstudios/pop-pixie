using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingRoom3Events : MonoBehaviour {
  public List<TradeSecretSprite> TradeSecrets;

  private bool RoomFinished = false;

  void Update() {
    if (StateManager.Isnt(State.Playing))
      return;

    if (!RoomFinished && TradeSecrets.All(x => x.Collected)) {
      RoomFinished = true;
      SceneManager.LoadScene("Training Room 4");
    }
  }
}
