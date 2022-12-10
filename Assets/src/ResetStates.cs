using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStates : MonoBehaviour {
  public List<State> States = new List<State>() { State.Playing };

  void Start() {
    StateManager.ResetStates(States);
  }
}
