using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigData : AData {
  public static ConfigData Current = new ConfigData();

  private bool loadData = true;

  public override void BeforeFetch() {
    if ( loadData && DataOperation().Exists() ) {
      loadData = false;
      DataOperation().Read();
    }
  }

  public override void AfterUpdate() {
    DataOperation().Write();
  }

  DataOperation DataOperation() {
    return new DataOperation( this, "config" );
  }

}
