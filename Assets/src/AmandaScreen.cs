using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmandaScreen : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public SpriteRenderer SpriteRenderer;
  public Sprite BrokenSprite;
  public GameObject OverrideGameObject;
  public float OverrideInterval;

  private float StartTime;
  private bool Destroyed = false;

  void Awake() {
    (OverrideHitPoints ?? GetComponent<HitPoints>()).OnBecomeZero.AddListener(hp => {
      SpriteRenderer.sprite = BrokenSprite;
      Destroyed = true;
    });

    StartTime = PlayingTime.time;
  }

  void Update() {
    if (OverrideGameObject != null) {
      float progress = ((PlayingTime.time - StartTime) / OverrideInterval) % 1f;
      OverrideGameObject.SetActive(!Destroyed && progress > 0.5);
    }
  }
}
