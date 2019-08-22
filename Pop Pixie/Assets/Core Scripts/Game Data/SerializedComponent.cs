using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedComponent {
  public Type Type;
  public Dictionary<string, dynamic> Fields;
}
