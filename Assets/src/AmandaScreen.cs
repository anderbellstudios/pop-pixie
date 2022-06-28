using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmandaScreen : MonoBehaviour, IHitPointEvents {
  public SpriteRenderer SpriteRenderer;
  public Sprite BrokenSprite;
  public GameObject OverrideGameObject;
  public int OverrideInterval;

  private IntervalTimer OverrideTimer;
  private bool Destroyed = false;

  public void Updated(HitPoints hp) {}
  public void Decreased(HitPoints hp) {}

  public void BecameZero(HitPoints hp) {
    SpriteRenderer.sprite = BrokenSprite;
    Destroyed = true;
  }

  void Awake() {
    OverrideTimer = new IntervalTimer() {
      TimeClass = "PlayingTime",
      Interval = OverrideInterval
    };

    OverrideTimer.Reset();
  }

  void Update() {
    if (OverrideGameObject != null) {
      OverrideGameObject.SetActive(!Destroyed && OverrideTimer.Progress() > 0.5);
      OverrideTimer.IfElapsed(() => OverrideTimer.Reset());
    }
  }
}
