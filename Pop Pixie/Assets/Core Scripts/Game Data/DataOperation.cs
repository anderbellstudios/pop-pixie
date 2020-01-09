using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataOperation {

  AData Data;
  string FileName;

  public DataOperation( AData data, string fileName ) {
    Data = data;
    FileName = fileName;
  }

  public void Write() {
    var bf = new BinaryFormatter();
    var file = File.Open( Path(), FileMode.OpenOrCreate );
    bf.Serialize(file, Data.Dictionary);
    file.Close();
  }

  public void Read() {
    var file = File.Open( Path(), FileMode.Open );
    var bf = new BinaryFormatter();

    try {
      Data.Dictionary = ( Dictionary<string, dynamic> ) bf.Deserialize( file );

    } catch (SerializationException e) {

      Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
      throw;

    } finally {

      file.Close();

    }
  }

  public bool Exists() {
    return File.Exists( Path() );
  }

  string Path() {
    return System.IO.Path.Combine(Application.persistentDataPath, FileName);
  }

}
