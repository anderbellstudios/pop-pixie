using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameDataController : MonoBehaviour {
  public static GameDataController Current;
  public GameData CurrentGame;

  void Awake () {
    DontDestroyOnLoad( gameObject );
    Current = this;
  }

  public void NewGame () {
    Debug.Log("Starting a new game");

    CurrentGame = new GameData();
    CurrentGame.LevelId = 1;

    Save();
  }

  public void Save () {
    Debug.Log("Attempting to save");
    Debug.Log( GameDataPath() );

    var bf = new BinaryFormatter();
    var file = File.Open( GameDataPath(), FileMode.OpenOrCreate );
    bf.Serialize(file, CurrentGame);
    file.Close();
  }

  private string GameDataPath () {
    return Application.persistentDataPath + "/game_data.dat";
  }
}
