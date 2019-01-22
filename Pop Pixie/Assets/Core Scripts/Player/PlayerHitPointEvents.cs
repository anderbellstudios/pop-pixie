using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitPointEvents : MonoBehaviour, IHitPointEvents {

  public MonoBehaviour HealthBar;

  public void Updated (HitPoints hp) {
    var hb = (HUDBar) HealthBar;
    hb.Progress = hp.Current / hp.Maximum;
  }

  public void Decreased (HitPoints hp) {
    var highlight = GameObject.Find("RedHighlight");
    var highlighter = highlight.GetComponent<RedHighlight>();

    highlighter.Flash();
  }

  public void BecameZero (HitPoints hp) {
    SceneManager.LoadScene("Game Over");
  }
}
