using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData : AData {
  public static GameData Current = new GameData();

  public override Dictionary<string, dynamic> LocalDefaultDictionary() {
    return new Dictionary<string, dynamic> {
      { "VERSION", "1.1" }
    };
  }

  public void Save() {
    Dictionary["scene"] = new SceneSerializer().Serialize();
  }

  public void Load() {
    GDCall.ExpectLoad();
    new SceneDeserializer( Dictionary["scene"] ).Deserialize();
  }

  public void WriteSave() {
    WriteTrueSave();
    WriteAutoSave();
  }

  public void WriteTrueSave() {
    Write("file0");
  }

  public void WriteAutoSave() {
    Write("file1");
  }

  public void ReadSave() {
    Read("file0");
  }

  public void ReadAutoSave() {
    Read("file1");
  }

  public void Write( string fileName ) {
    var bf = new BinaryFormatter();
    var file = File.Open( Path(fileName), FileMode.OpenOrCreate );
    bf.Serialize(file, Dictionary);
    file.Close();
  }

  public void Read( string fileName ) {
    var file = File.Open( Path(fileName), FileMode.Open );
    var bf = new BinaryFormatter();

    try {
      Dictionary = ( Dictionary<string, dynamic> ) bf.Deserialize( file );

    } catch (SerializationException e) {

      Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
      throw;

    } finally {

      file.Close();

    }
  }

  public bool Exists() {
    return File.Exists( Path("file1") );
  }

  string Path( string fileName ) {
    return System.IO.Path.Combine(Application.persistentDataPath, fileName);
  }

}
