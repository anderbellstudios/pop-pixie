using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BuildMetaData {
  public static String BranchName => Read("branch_name");
  public static String CommitHash => Read("commit_hash");

  public static void SetBranchName(string value) => Write("branch_name", value);

  public static void SetCommitHash(string value) => Write("commit_hash", value);

  public static void Reset() {
    SetBranchName(null);
    SetCommitHash(null);
  }

  private static void Write(string filename, String value) {
    File.WriteAllText($"Assets/Resources/{filename}.txt", value ?? "");
  }

  private static String Read(string filename) {
    string value = (Resources.Load(filename) as TextAsset).text.Trim();
    return value == "" ? null : value;
  }
}
