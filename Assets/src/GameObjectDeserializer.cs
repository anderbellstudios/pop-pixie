using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectDeserializer : MonoBehaviour {
  SerializedGameObject SerializedGameObject;

  public GameObjectDeserializer( SerializedGameObject serializedGameObject ) {
    SerializedGameObject = serializedGameObject;
  }

  public void Deserialize() {
    GameObject gameObject = GuidManager.ResolveGuid( SerializedGameObject.Guid );

    foreach ( SerializedComponent component in SerializedGameObject.Components ) {
      new ComponentDeserializer( component, gameObject ).Deserialize();
    }
  } 
}
