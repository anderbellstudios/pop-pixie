using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class DataOperation {
  AData Data;
  string FileName;

  public DataOperation(AData data, string fileName) {
    Data = data;
    FileName = fileName;
  }

  public void Write() {
    Data.BeforeWrite();
    File.WriteAllText(Path(), Data.Serialize());
  }

  public void Read() {
    string json = File.ReadAllText(Path());
    Data.Deserialize(json);
    Data.AfterRead();
  }

  public bool Exists() {
    return File.Exists(Path());
  }

  string Path() {
    return System.IO.Path.Combine(Application.persistentDataPath, FileName + ".json");
  }
}
