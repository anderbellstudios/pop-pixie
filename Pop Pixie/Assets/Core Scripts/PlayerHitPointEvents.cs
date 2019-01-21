using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitPointEvents : MonoBehaviour, IHitPointEvents {
  public void Decreased (HitPoints hp) {
    var highlight = GameObject.Find("RedHighlight");
    var highlighter = highlight.GetComponent<RedHighlight>();

    highlighter.Flash();
  }

  public void BecameZero (HitPoints hp) {
    GameDataController.Current.Load();
  }
}
