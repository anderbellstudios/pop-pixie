using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClearGameDataHopper : MonoBehaviour {
  public void Hop() {
    GameData.Reset();
  }
}
