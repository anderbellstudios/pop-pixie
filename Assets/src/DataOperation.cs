using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataOperation {

  AData Data;
  string FileName;

  public DataOperation(AData data, string fileName) {
    Data = data;
    FileName = fileName;
  }

  public void Write() {
    string json = JsonConvert.SerializeObject(Data.Dictionary);
    File.WriteAllText(Path(), json);
  }

  public void Read() {
    string json = File.ReadAllText(Path());
    Data.Dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
  }

  public bool Exists() {
    return File.Exists(Path());
  }

  string Path() {
    return System.IO.Path.Combine(Application.persistentDataPath, FileName + ".v2.json");
  }

}
