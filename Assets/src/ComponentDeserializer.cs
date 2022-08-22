using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentDeserializer : MonoBehaviour {
  SerializedComponent SerializedComponent;
  GameObject GameObject;

  public ComponentDeserializer(
    SerializedComponent serializedComponent,
    GameObject gameObject
  ) {
    SerializedComponent = serializedComponent;
    GameObject = gameObject;
  }

  public void Deserialize() {
    ISerializableComponent component = (ISerializableComponent) GameObject.GetComponent(
      SerializedComponent.Type
    );

    foreach (string fieldName in component.SerializableFields) {
      var field = SerializedComponent.Type.GetField(fieldName);
      dynamic val = SerializedComponent.Fields[fieldName];

      if (field.FieldType == typeof(Int32)) {
        field.SetValue(component, Convert.ToInt32(val));
      } else if (field.FieldType == typeof(Single)) {
        field.SetValue(component, (float) val);
      } else {
        field.SetValue(component, val);
      }

      if (component is ISaveCallbacks) {
        ((ISaveCallbacks) component).AfterLoad();
      }
    }
  } 
}
