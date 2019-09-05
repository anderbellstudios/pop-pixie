using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTransform : MonoBehaviour, ISerializableComponent, ISaveCallbacks {

  public string[] SerializableFields { get; } = { "x", "y", "z" };

  public Transform Transform;

  public float x, y, z;

  public void BeforeSave() {
    x = Transform.position.x;
    y = Transform.position.y;
    z = Transform.position.z;
  }

  public void AfterLoad() {
    Transform.position = new Vector3( x, y, z );
  }

}
