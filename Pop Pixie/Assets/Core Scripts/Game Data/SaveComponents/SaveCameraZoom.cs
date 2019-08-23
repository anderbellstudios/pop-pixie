using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCameraZoom : MonoBehaviour, ISerializableComponent, ISaveCallbacks {

  public string[] SerializableFields { get; } = { "size" };

  public Camera Camera;

  public float size;

  public void BeforeSave() {
    size = Camera.orthographicSize;
  }

  public void AfterLoad() {
    Camera.orthographicSize = size;
  }

}
