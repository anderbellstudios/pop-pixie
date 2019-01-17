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
    if ( Path != null && ShouldApproach() ) {
      Vector3 heading = Path[0] - transform.position;
      rb.MovePosition(transform.position + heading.normalized * Speed * Time.deltaTime);

      if ( heading.magnitude < 0.2 )
        CalculatePath();

    } else {
      rb.velocity = Vector3.zero;
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
