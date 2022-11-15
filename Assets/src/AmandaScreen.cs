using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmandaScreen : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public SpriteRenderer SpriteRenderer;
  public Sprite BrokenSprite;
  public GameObject OverrideGameObject;
  public int OverrideInterval;

  private IntervalTimer OverrideTimer;
  private bool Destroyed = false;

  void Awake() {
    (OverrideHitPoints ?? GetComponent<HitPoints>()).OnBecomeZero.AddListener(hp => {
      SpriteRenderer.sprite = BrokenSprite;
      Destroyed = true;
    });

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
