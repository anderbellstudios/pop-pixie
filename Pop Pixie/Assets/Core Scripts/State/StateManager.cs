using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State : int { 
  Playing, 
  Paused,
  Dialogue, 
  Lore, 
  DialoguePrompt, 
  Dying,
  LoadingLevel,
  Cutscene,
  ScriptedMovement
};

public class StateManager : MonoBehaviour {
  public static int State;

  public bool SetInitialState = false;
  public State InitialState = global::State.Playing;

  void Awake() {
    if ( SetInitialState )
      StateManager.SetState( InitialState );
  }

  public static bool Is(object state) {
    return State == (int)state;
  }

  public static bool Isnt(object state) {
    return !Is(state);
  }

  public static void SetState (object state) {
    State = (int)state;
  }
}
