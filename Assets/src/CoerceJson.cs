using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CoerceJson {
  public static T To<T>(dynamic obj) {
    if (obj is T) {
      return (T)obj;
    } else if (obj is JObject) {
      return ((JObject)obj).ToObject<T>();
    } else if (obj is JArray) {
      return ((JArray)obj).ToObject<T>();
    }

    throw new ArgumentException("Unexpected JSON-like object: " + obj.ToString());
  }
}
