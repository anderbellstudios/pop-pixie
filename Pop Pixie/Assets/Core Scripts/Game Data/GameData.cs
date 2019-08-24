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
    GDCall.ExpectLoad();
    new SceneDeserializer( Current.Dictionary["scene"] ).Deserialize();
  }

  public static void WriteSave() {
    Write("file0");
    Write("file1");
  }

  public static void WriteAutoSave() {
    Write("file1");
  }

  public static void ReadSave() {
    Read("file0");
  }

  public static void ReadAutoSave() {
    Read("file1");
  }

  public static void Write( string fileName ) {
    var bf = new BinaryFormatter();
    var file = File.Open( Path(fileName), FileMode.OpenOrCreate );
    bf.Serialize(file, Current.Dictionary);
    file.Close();
  }

  public static void Read( string fileName ) {
    var file = File.Open( Path(fileName), FileMode.Open );
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
    return File.Exists( Path("file1") );
  }

  static string Path( string fileName ) {
    return Application.persistentDataPath + fileName;
  }

}
