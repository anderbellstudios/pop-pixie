using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AData {
  public Dictionary<string, object> Dictionary;

  public AData() {
    Clear();
  }

  public void Clear() {
    Debug.Log("Clearing");
    Dictionary = LocalDefaultDictionary();
  }

  public virtual Dictionary<string, object> LocalDefaultDictionary() {
    return new Dictionary<string, object>();
  }

  public dynamic Fetch(string key, object orSetEqualTo = null) {
    BeforeFetch();

    if (Dictionary.ContainsKey(key))
      return Dictionary[key];

    if (orSetEqualTo != null)
      return Dictionary[key] = orSetEqualTo;

    return null;
  }

  public void Set(string key, object val) {
    Dictionary[key] = val;
    AfterUpdate();
  }

  public virtual void BeforeFetch() {
  }

  public virtual void AfterUpdate() {
  }
}
