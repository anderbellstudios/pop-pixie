using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour, IHitPointEvents {
  public SpriteRenderer SpriteRenderer;
  public Sprite[] WorkingFrames;
  public Sprite DestroyedFrame;

  public bool DisableColliderOnBreak;
  public Behaviour Collider;

  public void Updated (HitPoints hp) {
    if ( hp.Current == 0 ) {
      SetSprite( DestroyedFrame );
    } else {
      int frames_count = WorkingFrames.Length;
      float increment = hp.Maximum / frames_count;
      int frame_no = frames_count - (int) Math.Ceiling( hp.Current / increment );
      SetSprite( WorkingFrames[frame_no] );
    }
  }

  void SetSprite( Sprite sprite ) {
    SpriteRenderer.sprite = sprite;
  }

  public void Decreased (HitPoints hp) {
  }

  public void BecameZero (HitPoints hp) {
    if ( DisableColliderOnBreak )
      Collider.enabled = false;
  }

}
