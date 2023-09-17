using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LoreItem")]
[System.Serializable]
public class LoreItem : ScriptableObject {
  public string UniqueId = null;
  public string Name;
  public Sprite Image;
}
