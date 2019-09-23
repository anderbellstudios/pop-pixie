using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3BossPhase : APhase, ISerializableComponent {

  public string[] SerializableFields { get; } = { "JumpedDown" };

  public Level3JumpDownAnimation JumpAnimation;
  public HitPoints HitPoints;
  public Rigidbody2D rb;
  public AEnemyAI InitialAI;

  public bool JumpedDown = false;

	public override void LocalBegin () {
    if ( JumpedDown ) {
      // Loaded autosave after jump
      StartAI();
    } else {
      JumpAnimation.Perform( JumpFinished );
    }
  }

  void JumpFinished() {
    Invoke( "StartAI", 0.5f );
    JumpedDown = true;
  }

  void StartAI() {
    rb.isKinematic = false;
    InitialAI.GainControl();
  }

  public override float ProgressBarAllotment() {
    return 1f;
  }

  public override float ProgressBarValue() {
    return HitPoints.Current / HitPoints.Maximum;
  }

}
