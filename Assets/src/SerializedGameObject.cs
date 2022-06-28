using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedGameObject {
  public Guid Guid;
  public SerializedComponent[] Components;
}
