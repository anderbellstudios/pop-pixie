using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmandaScreen : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public SpriteRenderer SpriteRenderer;
  public Sprite BrokenSprite;
  public GameObject OverrideGameObject;
  public float OverrideInterval;

  private Stopwatch OverrideStopwatch;
  private bool Destroyed = false;

  void Awake() {
    (OverrideHitPoints ?? GetComponent<HitPoints>()).OnBecomeZero.AddListener(hp => {
      SpriteRenderer.sprite = BrokenSprite;
      Destroyed = true;
    });

    OverrideStopwatch = new Stopwatch.PlayingTime();
  }

  void Update() {
    if (OverrideGameObject != null) {
      float progress = OverrideStopwatch.LoopedProgress(OverrideInterval);
      OverrideGameObject.SetActive(!Destroyed && progress > 0.5);
    }
  }
}
