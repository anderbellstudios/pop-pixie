using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData {
  public static GameData Current = new GameData();

  public Dictionary<string, dynamic> Dictionary = new Dictionary<string, dynamic> {
    { "VERSION", "1.1" }
  };

  public static void Save() {
    Current.Dictionary["scene"] = new SceneSerializer().Serialize();
  }

  public static void Load() {
    new SceneDeserializer( Current.Dictionary["scene"] ).Deserialize();
  }

  public static void Write() {
    var bf = new BinaryFormatter();
    var file = File.Open( Path(), FileMode.OpenOrCreate );
    bf.Serialize(file, Current.Dictionary);
    file.Close();
  }

  public static void Read() {
    var file = File.Open( Path(), FileMode.Open );
    var bf = new BinaryFormatter();

    try {
      Current.Dictionary = ( Dictionary<string, dynamic> ) bf.Deserialize( file );

    } catch (SerializationException e) {

      Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
      throw;

    } finally {

      file.Close();

    }
  }

  public static bool Exists() {
    return File.Exists( Path() );
  }

  static string Path () {
    return Application.persistentDataPath + "/file0";
  }

}
