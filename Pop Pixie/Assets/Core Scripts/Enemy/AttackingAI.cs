using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingAI : MonoBehaviour {

  public float Speed;
  public float Damage;
  public SoundController SoundPlayer;
  public List<AudioClip> Sounds;

  private Rigidbody2D rb;
  private GameObject target;

	// Use this for initialization
	void Start () {
    rb = gameObject.GetComponent<Rigidbody2D>();
    target = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
    if ( StateManager.Isnt( State.Playing ) ) {
      rb.velocity = Vector3.zero;
      return;
    }

    Vector3 direction = ( target.transform.position - transform.position ).normalized;
    rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
	}

  void OnCollisionStay2D (Collision2D col) {
    // Make sure AI is enabled
    if ( !this.enabled )
      return;

    var obj = col.gameObject;

    // If body is player,
    if ( obj.name == "Pixie" ) {
      // Do damage
      obj.GetComponent<HitPoints>().Damage( Damage );

      // Play attack sound
      int i = UnityEngine.Random.Range(0, Sounds.Count);
      var sound = Sounds[i];
      SoundPlayer.Play(sound);

      // Clear cooldown timer, ending Attacking AI
      gameObject.GetComponent<EnemyAI>().ResetCoolDownTimer();
    }
  }
}
