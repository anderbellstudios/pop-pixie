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

     Debug.Log("Found a route:");
     Vector3[] vertices = pathfinder.Vertices();
     foreach (Vector3 v in vertices) {
       Debug.Log(v);
     }

     enemy.GetComponent<Rigidbody2D>().velocity = ( vertices[0] - start );
   }
 }

}
