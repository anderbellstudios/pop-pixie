using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigData : AData {
  public static ConfigData Current = new ConfigData();

  public override void BeforeFetch() {
    if ( DataOperation().Exists() )
      DataOperation().Read();
  }

  public override void AfterUpdate() {
    DataOperation().Write();
  }

  DataOperation DataOperation() {
    return new DataOperation( this, "config" );
  }

}
