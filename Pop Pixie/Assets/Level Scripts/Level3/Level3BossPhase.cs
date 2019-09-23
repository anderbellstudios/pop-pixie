using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3BossPhase : APhase, ISerializableComponent {

  public string[] SerializableFields { get; } = { "JumpedDown" };

  public Level3JumpDownAnimation JumpAnimation;
  public Rigidbody2D rb;
  public AEnemyAI InitialAI;

  public bool JumpedDown = false;

	public override void LocalBegin () {
    if ( JumpedDown ) {
      // Loaded autosave after jump
      Debug.Log("Skipping jump");
      StartAI();
    } else {
      Debug.Log("Jumping down");
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

}
