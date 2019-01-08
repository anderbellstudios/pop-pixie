using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolingDownAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    var enemy  = gameObject;
    var player = GameObject.FindGameObjectWithTag("Player");
    var start       = enemy.transform.position;
    var destination = player.transform.position;
    var radius = enemy.GetComponent<CircleCollider2D>().radius;

    var pathfinder = new AndersonsAlgorithm(
      start:       start,
      destination: destination,
      radius: radius
    );

    Vector3[] vertices = pathfinder.Vertices();

    enemy.GetComponent<Rigidbody2D>().velocity = 4.0f * Vector3.Normalize( vertices[0] - start );
	}
}
