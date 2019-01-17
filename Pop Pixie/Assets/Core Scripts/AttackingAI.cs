using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingAI : MonoBehaviour {

  public float Speed;
  public float Damage;

  private Rigidbody2D rb;
  private GameObject target;

	// Use this for initialization
	void Start () {
    rb = gameObject.GetComponent<Rigidbody2D>();
    target = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
    Vector3 direction = ( target.transform.position - transform.position ).normalized;
    rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
	}

  void OnCollisionStay2D (Collision2D col) {
    var obj = col.gameObject;

    // On valid collisions with player,
    if ( obj.name == "Pixie" && this.enabled ) {
      // Do damage
      obj.GetComponent<PlayerHitPoints>().Decrease( Damage );

      // And clear cooldown timer, ending Attacking AI
      gameObject.GetComponent<EnemyAI>().ResetCoolDownTimer();
    }
  }
}
