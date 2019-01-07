using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof(EnemyAI) )]
class AndersonsAlgorithmTester : Editor {

 public override void OnInspectorGUI() {
   DrawDefaultInspector();

   if ( GUILayout.Button("Test Pathfinding") ) {
     var pathfinder = new AndersonsAlgorithm(
       start:       new Vector2( 0.0f, 7.5f ),
       destination: new Vector2( 0.0f, 0.0f ),
       radius: 0.5f
     );

     pathfinder.Vertices();
   }
 }

}
