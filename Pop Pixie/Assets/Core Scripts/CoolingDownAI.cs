using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolingDownAI : MonoBehaviour {

  public float Speed;
  public float ApproachDistance;

  private Vector3[] Path;
  private Rigidbody2D rb;
  private GameObject target;

	// Use this for initialization
	void Start () {
    rb = gameObject.GetComponent<Rigidbody2D>();
    target = GameObject.FindGameObjectWithTag("Player");
    Path = null;

    // Calculate the path every half-second
    InvokeRepeating("CalculatePath", 0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
    if ( !ShouldApproach() ) {
      // Stay still
      rb.velocity = Vector3.zero;
      return;
    }

    if ( Path != null ) {

      Vector3 heading = Path[0] - transform.position;
      rb.MovePosition(transform.position + heading.normalized * Speed * Time.deltaTime);

      if ( heading.magnitude < 0.2 )
        CalculatePath();

    } else {

      // Move away from player
      Vector3 direction = -1 * ( target.transform.position - transform.position ).normalized;
      rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);

      CalculatePath();

    }
	}

  private bool ShouldApproach() {
    Vector3 heading = target.transform.position - transform.position;
    float distance = heading.magnitude;

    bool lineOfSight = false;

    RaycastHit2D[] hits = new RaycastHit2D[1];
    gameObject.GetComponent<CircleCollider2D>().Raycast(heading, hits);

    if (hits.Length > 0) {
      if (hits[0].transform == target.transform) {
        lineOfSight = true;
      }
    }

    // Approach if too far away or cannot see target
    return (distance > ApproachDistance) || !lineOfSight;
  }

  private void CalculatePath() {
    var radius = gameObject.GetComponent<CircleCollider2D>().radius;

    var pathfinder = new AndersonsAlgorithm(
      start:       transform.position,
      destination: target.transform.position,
      radius: radius
    );

    Path = pathfinder.Vertices();
  }
}
