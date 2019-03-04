using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State : int { Playing, Dialogue };

public class StateManager : MonoBehaviour {
  public static int State;

  public static void SetState (object state) {
    State = (int)state;
  }
}
