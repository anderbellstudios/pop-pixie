using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHitPointEvents : MonoBehaviour, IHitPointEvents {
  public SoundController SoundPlayer;
  public List<AudioClip> Sounds;

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
    StartCoroutine( Flash() );

    // Play hurt sound
    int i = UnityEngine.Random.Range(0, Sounds.Count);
    var sound = Sounds[i];
    SoundPlayer.Play(sound);
  }

  private IEnumerator Flash() {
    var renderer = gameObject.GetComponent<SpriteRenderer>();

    for(var n = 0; n < 10; n++) {
      renderer.enabled = true;
      yield return new WaitForSeconds(0.1f);
      renderer.enabled = false;
      yield return new WaitForSeconds(0.1f);
    }

    renderer.enabled = true;
  }

  public void BecameZero (HitPoints hp) {
    Destroy(gameObject);
  }
}
