using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameObject : MonoBehaviour {

  public static GameObject Current;

  void Awake() {
    Current = gameObject;
  }

}
