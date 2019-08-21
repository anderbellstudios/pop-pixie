using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectSerializer {
  private GameObject GameObject;

  public GameObjectSerializer( GameObject go ) {
    GameObject = go;
  }

  public SerializedGameObject Serialize() {
    var guidComponent = GameObject.GetComponent<GuidComponent>();

    if ( guidComponent == null )
      return null;

    foreach ( var component in SerializableComponents() ) {
      if ( component is ISaveCallbacks ) {
        ( (ISaveCallbacks) component ).BeforeSave();
      }
    }

    return new SerializedGameObject() {
      Guid = guidComponent.GetGuid(),
      Components = SerializableComponents()
    };
  }

  Component[] SerializableComponents() {
    return GameObject.GetComponents<Component>().ToList().Where(
      component => component is ISerializableComponent
    ).ToArray();
  }
}
