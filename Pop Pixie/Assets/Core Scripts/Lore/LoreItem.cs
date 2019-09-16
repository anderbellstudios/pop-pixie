using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class LoreItem {
  public string Text;
  public string UniqueId = null;

  public string Name {
    get {
      return CultureInfo.CurrentCulture.TextInfo.ToTitleCase( UniqueId ).Replace("-", " ");
    }
  }

  public static LoreItem ParseJSON (string json) {
    return JsonUtility.FromJson<LoreItem>(json);
  }
}
