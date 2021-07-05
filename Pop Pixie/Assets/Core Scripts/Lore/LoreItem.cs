using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class LoreItem {
  public string UniqueId = null;
  public string Name;
  [TextArea]
  public string Text;
}
