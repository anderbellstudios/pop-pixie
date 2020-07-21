using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleted : MonoBehaviour {

  public ElevatorBarrier ElevatorBarrier;

  public void Run() {
    ElevatorBarrier.Remove();
	}
}
