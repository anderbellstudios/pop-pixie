using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitPointEvents : MonoBehaviour, IHitPointEvents {

  public SoundController SoundPlayer;
  public float FlashDuration; 
  public DeathAnimation DeathAnimation;
  public Collider2D Collider;
  public List<AudioClip> Sounds;

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
    if ( FlashDuration > 0 ) {
      float duration = FlashDuration;
      StartCoroutine( Flash(duration) );
    }

    if ( Sounds.Count > 0 ) {
      // Play hurt sound
      int i = UnityEngine.Random.Range(0, Sounds.Count);
      var sound = Sounds[i];
      SoundPlayer.Play(sound);
    }
  }

  private IEnumerator Flash(float duration) {
    var renderer = gameObject.GetComponent<SpriteRenderer>();

    int flashes = (int)( duration / 0.2f );

    for(var n = 0; n < flashes; n++) {
      renderer.enabled = true;
      yield return new WaitForSeconds(0.1f);
      renderer.enabled = false;
      yield return new WaitForSeconds(0.1f);
    }

    renderer.enabled = true;
  }

  public void BecameZero (HitPoints hp) {
    disableAIs();
    Collider.enabled = false;
    DeathAnimation.Play();
  }

  void disableAIs() {
    foreach ( var ai in GetComponents<AEnemyAI>() ) {
      if ( ai.InControl )
        ai.RelinquishControl();
    }
  }

}
