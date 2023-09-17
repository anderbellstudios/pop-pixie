using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectSerializer {
  private GameObject GameObject;

  public GameObjectSerializer(GameObject go) {
    GameObject = go;
  }

  public SerializedGameObject Serialize() {
    var guidComponent = GameObject.GetComponent<GuidComponent>();

    if (guidComponent == null)
      return null;

    return new SerializedGameObject() {
      Guid = guidComponent.GetGuid(),
      Components = SerializedComponents()
    };
  }

  SerializedComponent[] SerializedComponents() {
    return SerializableComponents().ToList().Select(
      component => new ComponentSerializer(component).Serialize()
    ).ToArray();
  }

  ISerializableComponent[] SerializableComponents() {
    return GameObject.GetComponents<Component>().ToList().Where(
      component => component is ISerializableComponent
    ).ToList().Select(
      component => (ISerializableComponent)component
    ).ToArray();
  }
}
