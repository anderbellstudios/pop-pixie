using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class AData {
  public Dictionary<string, object> Dictionary;
  public UnityEvent OnChange = new UnityEvent();

  public AData() {
    Clear();
  }

  public void Clear() {
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
    OnChange.Invoke();
    AfterUpdate();
  }

  public virtual void BeforeWrite() {
  }

  public virtual void AfterRead() {
  }

  public virtual void BeforeFetch() {
  }

  public virtual void AfterUpdate() {
  }
}
