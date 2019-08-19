using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour, IHitPointEvents {
  public SpriteRenderer SpriteRenderer;
  public Sprite[] WorkingFrames;
  public Sprite DestroyedFrame;

  public void Updated (HitPoints hp) {
  }

  public void Decreased (HitPoints hp) {
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

  public void BecameZero (HitPoints hp) {
  }

}
