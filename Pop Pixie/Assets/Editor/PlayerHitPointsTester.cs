using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof(PlayerHitPoints) )]
class PlayerHitPointsTester : Editor {

 public override void OnInspectorGUI() {
   DrawDefaultInspector();

   var hp = (PlayerHitPoints) target;

   if ( GUILayout.Button("Do damage") ) {
     Debug.Log( hp.Decrease(5.0f) );
   }

   if ( GUILayout.Button("Heal damage") ) {
     Debug.Log( hp.Increase(3.0f) );
   }

   if ( GUILayout.Button("Set HP") ) {
     Debug.Log( hp.Set(15.0f) );
   }
 }

}
