using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour {
  public HitPoints OverrideHitPoints;
  public SpriteRenderer SpriteRenderer;
  public Sprite[] WorkingFrames;
  public Sprite DestroyedFrame;
  public SpawnFlyingRingPull SpawnFlyingRingPull;
  public List<Behaviour> DisableComponents;

  bool StoppedWorking = false;

  void Awake() {
    HitPoints hitPoints = OverrideHitPoints ?? GetComponent<HitPoints>();

    hitPoints.OnUpdate.AddListener(hp => {
      if (hp.Current == 0) {
        if (!StoppedWorking) StopWorking();
        SetSprite(DestroyedFrame);
      } else {
        int frames_count = WorkingFrames.Length;
        float increment = hp.Maximum / frames_count;
        int frame_no = frames_count - (int) Math.Ceiling(hp.Current / increment);
        SetSprite(WorkingFrames[frame_no]);
      }
    });

    hitPoints.OnBecomeZero.AddListener(hp => {
      if (SpawnFlyingRingPull != null) {
        SpawnFlyingRingPull.Instantiate();
      }
    });
  }

  void SetSprite(Sprite sprite) {
    SpriteRenderer.sprite = sprite;
  }

  void StopWorking() {
    StoppedWorking = true;
    DisableComponents.ForEach( comp => comp.enabled = false );
  }
}
