using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataController : MonoBehaviour {
  public static GameDataController Current;

  void Awake () {
    DontDestroyOnLoad( gameObject );
    Current = this;
  }

  public void NewGame () {
    Debug.Log("Starting a new game");

    var gameData = new GameData();
    gameData.LevelId = 1;
    gameData.MakeCurrent();
  }
}
