using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof(EnemyAI) )]
class AndersonsAlgorithmTester : Editor {

 public override void OnInspectorGUI() {
   DrawDefaultInspector();

   if ( GUILayout.Button("Test Pathfinding") ) {
     var enemy  = GameObject.FindGameObjectWithTag("Enemy");
     var player = GameObject.FindGameObjectWithTag("Player");
     var start       = enemy.transform.position;
     var destination = player.transform.position;
     var radius = enemy.GetComponent<CircleCollider2D>().radius;

     var pathfinder = new AndersonsAlgorithm(
       start:       start,
       destination: destination,
       radius: radius
     );

     pathfinder.Vertices();
   }
 }

}
