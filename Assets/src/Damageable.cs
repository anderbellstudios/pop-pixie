using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour {
  public PersistentId PersistentId;
  public HitPoints OverrideHitPoints;
  public SpriteRenderer SpriteRenderer;
  public Sprite[] WorkingFrames;
  public Sprite DestroyedFrame;
  public SpawnFlyingRingPull SpawnFlyingRingPull;
  public List<Behaviour> DisableComponents;
  public UnityEvent OnStopWorking;

  private bool StoppedWorking = false;
  private HitPoints HitPoints => OverrideHitPoints ?? GetComponent<HitPoints>();
  private string Id => PersistentId.Id;

  void Awake() {
    if (DisableComponents.Count > 0) {
      Debug.LogError("Damageable.DisableComponents is deprecated. Use OnStopWorking instead.");
    }

    OrderedStart.Add(() => {
      if (ActivatedData.IsActivated(Id)) {
        HitPoints.Current = 0;
        HitPoints.Dead = true;
      }

      HitPoints.OnUpdate.AddListener(hp => {
        if (hp.Current == 0) {
          StopWorking();
        } else {
          int frames_count = WorkingFrames.Length;
          float increment = hp.Maximum / frames_count;
          int frame_no = frames_count - (int)Math.Ceiling(hp.Current / increment);
          SetSprite(WorkingFrames[frame_no]);
        }
      });

      HitPoints.OnBecomeZero.AddListener(hp => {
        if (SpawnFlyingRingPull != null) {
          SpawnFlyingRingPull.Instantiate();
        }
      });
    }, OrderedStart.Between(
      HitPoints.InitStartOrder,
      HitPoints.UpdateStartOrder
    ));
  }

  private void StopWorking() {
    if (StoppedWorking)
      return;
    StoppedWorking = true;
    ActivatedData.RecordActivation(Id);
    SetSprite(DestroyedFrame);
    DisableComponents.ForEach(comp => comp.enabled = false);
    OnStopWorking.Invoke();
  }

  private void SetSprite(Sprite sprite) {
    SpriteRenderer.sprite = sprite;
  }
}
