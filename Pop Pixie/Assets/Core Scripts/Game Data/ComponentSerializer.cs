using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentSerializer {
  private ISerializableComponent Component;

  public ComponentSerializer( ISerializableComponent component ) {
    Component = component;
  }

  public SerializedComponent Serialize() {
    if ( Component is ISaveCallbacks ) {
      ( (ISaveCallbacks) Component ).BeforeSave();
    }

    return new SerializedComponent() {
      Type = Component.GetType(),
      Fields = Fields()
    };
  }

  Dictionary<string, object> Fields() {
    var dictionary = new Dictionary<string, object>();

    foreach ( string fieldName in Component.SerializableFields ) {
      var field = Component.GetType().GetField( fieldName );
      object val = field.GetValue( Component );
      dictionary.Add( fieldName, val );
    }

    return dictionary;
  }
}
