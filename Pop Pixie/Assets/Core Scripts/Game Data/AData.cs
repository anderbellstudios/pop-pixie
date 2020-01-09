using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AData {
  public Dictionary<string, dynamic> Dictionary;

  public AData() {
    Dictionary = LocalDefaultDictionary();
  }

  public virtual Dictionary<string, dynamic> LocalDefaultDictionary() {
    return new Dictionary<string, dynamic>();
  }

  public dynamic Fetch( string key, dynamic orSetEqualTo = null ) {
    BeforeFetch();

    if ( Dictionary.ContainsKey(key) )
      return Dictionary[key];

    if ( orSetEqualTo != null )
      return Dictionary[key] = orSetEqualTo;

    return null;
  }

  public void Set( string key, dynamic val ) {
    Dictionary[key] = val;
    AfterUpdate();
  }

  public virtual void BeforeFetch() {
  }

  public virtual void AfterUpdate() {
  }

}
