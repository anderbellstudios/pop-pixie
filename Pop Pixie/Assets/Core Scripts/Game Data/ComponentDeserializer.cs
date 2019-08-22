using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentDeserializer : MonoBehaviour {
  SerializedComponent SerializedComponent;
  GameObject GameObject;

  public ComponentDeserializer( SerializedComponent serializedComponent, GameObject gameObject ) {
    SerializedComponent = serializedComponent;
    GameObject = gameObject;
  }

  public void Deserialize() {
    dynamic component = GameObject.GetComponent( SerializedComponent.Type );

    foreach ( string fieldName in ( ( ISerializableComponent ) component ).SerializableFields ) {
      var field = SerializedComponent.Type.GetField( fieldName );
      dynamic val = SerializedComponent.Fields[ fieldName ];
      field.SetValue( component, val );

      if ( component is ISaveCallbacks ) {
        ( (ISaveCallbacks) component ).AfterLoad();
      }

    }
  } 
}
