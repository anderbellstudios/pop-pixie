using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWhenPlayerBehind : MonoBehaviour {
  public SpriteRenderer SpriteRenderer;
  public float TransparentOpacity, Duration;

  private bool IsBehind = false;
  private float TargetOpacity => IsBehind ? TransparentOpacity : 1f;

  void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      IsBehind = true;
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (other.tag == "Player") {
      IsBehind = false;
    }
  }

  void Update() {
    Opacity = Mathf.MoveTowards(Opacity, TargetOpacity, Time.deltaTime / Duration);
  }

  private float Opacity {
    get {
      return SpriteRenderer.color.a;
    }

    set {
      if (Opacity != value) {
        SpriteRenderer.color = new Color(1f, 1f, 1f, value);
      }
    }
  }
}
