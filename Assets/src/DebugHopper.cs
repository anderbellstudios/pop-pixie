using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHopper : MonoBehaviour {
  public string Message;

  public void Hop() {
    Debug.Log(Message);
  }
}
