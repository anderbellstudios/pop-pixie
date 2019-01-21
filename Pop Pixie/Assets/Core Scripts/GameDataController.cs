using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
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

    Save();
  }

  public void Save () {
    Debug.Log("Attempting to save");
    Debug.Log( GameDataPath() );

    var gameData = GameData.Current;
    var bf = new BinaryFormatter();
    var file = File.Open( GameDataPath(), FileMode.OpenOrCreate );
    bf.Serialize(file, gameData);
    file.Close();
  }

  private string GameDataPath () {
    return Application.persistentDataPath + "/game_data.dat";
  }
}
