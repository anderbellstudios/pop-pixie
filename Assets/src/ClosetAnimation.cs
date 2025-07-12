using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClosetAnimation : APhase {
  public Door Door;
  public float DelayBeforeRoll;
  public float RollDistance,
    RollSpeed;

  public override bool SkipOnRetry() {
    DelayBeforeRoll = 0f;
    return false;
  }

  public override void LocalBegin() {
    StateManager.AddState(State.ScriptedMovement);
    AsyncTimer.BaseTime.SetTimeout(RollOutOfCloset, DelayBeforeRoll);
  }

  public override void AfterFinished() {
    StateManager.RemoveState(State.ScriptedMovement);
    Door.Close();
  }

  private void RollOutOfCloset() {
    Door.Open();

    GameObject player = PlayerGameObject.Current;
    ScriptedMovement scriptedMovement = player.GetComponent<ScriptedMovement>();
    Roll roll = player.GetComponentInChildren<Roll>();

    AsyncTimer.BaseTime.SetTimeout(
      () => {
        roll.StartRolling();

        scriptedMovement.FollowPath(
          new List<Vector3>() { player.transform.position + (RollDistance * Vector3.right) },
          RollSpeed,
          PhaseFinished
        );
      },
      0.1f
    );
  }
}
