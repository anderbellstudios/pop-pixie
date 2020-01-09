using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame {

  public static void WriteSave() {
    WriteTrueSave();
    WriteAutoSave();
  }

  public static void WriteTrueSave() {
    Write("file0");
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

  public static bool Exists() {
    return new DataOperation( GameData.Current, "file1" ).Exists();
  }

  static void Write( string fileName ) {
    new DataOperation( GameData.Current, fileName ).Write();
  }

  static void Read( string fileName ) {
    new DataOperation( GameData.Current, fileName ).Read();
  }

}
