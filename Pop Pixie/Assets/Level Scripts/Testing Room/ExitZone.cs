using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitZone : MonoBehaviour {

  public SpriteRenderer SpriteRenderer;
  public Color InactiveColor, ActiveColor;
  public float Delay;

  void OnTriggerEnter2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      SpriteRenderer.color = ActiveColor;
      Invoke("Leave", Delay);
    }
  }

  void OnTriggerExit2D (Collider2D other) {
    if ( other.tag == "Player" ) {
      SpriteRenderer.color = InactiveColor;
      CancelInvoke();
    }
  }

  void Leave() {
    SceneManager.LoadScene("Shop");
  }

}
