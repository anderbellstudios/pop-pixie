using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

public abstract class AData {
  public class MemorySave {
    public string Json;
  }

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

  public string Serialize() {
    return JsonConvert.SerializeObject(Dictionary);
  }

  public void Deserialize(string json) {
    Dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
  }

  public MemorySave ToMemorySave() {
    return new MemorySave() {
      Json = Serialize()
    };
  }

  public void LoadMemorySave(MemorySave save) {
    Deserialize(save.Json);
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
