using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof(SaveLoadHelper) )]
class SaveLoadHelperButtons : Editor {

 public override void OnInspectorGUI() {
   DrawDefaultInspector();

   if ( GUILayout.Button("Save") ) {
     SceneData.Save();
   }

   if ( GUILayout.Button("Load") ) {
     SceneData.Load();
   }
 }

}
