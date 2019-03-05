using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataController : MonoBehaviour {
  public static GameDataController Current;
  public GameData CurrentGame;

  void Awake () {
    if ( Current == null ) {
      Current = this;
    } else {
      Destroy( gameObject );
    }

    DontDestroyOnLoad( gameObject );
  }

  public void NewGame () {
    Debug.Log("Starting a new game");

    CurrentGame = new GameData();
    CurrentGame.LevelId = 1;

    Save();

    SceneManager.LoadScene( CurrentGame.LevelId );
  }

  public void NextLevel () {
    CurrentGame.LevelId += 1;
    Save();
    SceneManager.LoadScene( CurrentGame.LevelId );
  }

  public bool GameDataExists () {
    return File.Exists( GameDataPath() );
  }

  public void Save () {
    Debug.Log("Attempting to save");
    Debug.Log( GameDataPath() );

    var bf = new BinaryFormatter();
    var file = File.Open( GameDataPath(), FileMode.OpenOrCreate );
    bf.Serialize(file, CurrentGame);
    file.Close();
  }

  public void Load () {
    var file = File.Open( GameDataPath(), FileMode.Open );
    var bf = new BinaryFormatter();

    try {
      CurrentGame = (GameData) bf.Deserialize( file );

    } catch (SerializationException e) {

      Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
      throw;

    } finally {

      file.Close();

    }

    SceneManager.LoadScene( CurrentGame.LevelId );
  }

  private string GameDataPath () {
    return Application.persistentDataPath + "/game_data.dat";
  }
}
