using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigData : AData {
  public static ConfigData Current = new ConfigData();

  private bool LoadData = true;

  public override void BeforeFetch() {
    if (LoadData && ConfigDataOperation.Exists()) {
      LoadData = false;
      ConfigDataOperation.Read();
    }
  }

  public override void AfterUpdate() {
    ConfigDataOperation.Write();
  }

  private DataOperation ConfigDataOperation => new DataOperation(this, "config");
}
