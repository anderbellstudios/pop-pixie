using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClosetAnimation : APhase {
  public GameObject PlayerSpriteGroup;
  public Transform RattleTransform;
  public Sprite ClosetOpenSprite;
  public SpriteRenderer ClosetSpriteRenderer, DoorSpriteRenderer;
  public BoxCollider2D ClosetCollider;
  public float RattleAmplitude;
  public float RollDistance, RollSpeed;
  public List<float> RattleOnOffDurations;

  private bool Rattling = false;
  private int RattleOnOffIndex = 0;

  void Awake() {
    PlayerSpriteGroup.SetActive(false);
  }

  public override bool SkipOnRetry() {
    RollOutOfCloset();
    return true;
  }

  public override void LocalBegin() {
    StateManager.AddState(State.ScriptedMovement);
    NextRattleOnOff();
  }

  public override void WhilePhaseRunning() {
    if (Rattling) {
      float randomAngle = Random.Range(-RattleAmplitude, RattleAmplitude);
      RattleTransform.rotation = Quaternion.Euler(0, 0, randomAngle);
    } else {
      RattleTransform.rotation = Quaternion.identity;
    }
  }

  public override void AfterFinished() {
    StateManager.RemoveState(State.ScriptedMovement);
    ClosetCollider.enabled = true;
  }

  void NextRattleOnOff() {
    if (RattleOnOffIndex >= RattleOnOffDurations.Count) {
      Rattling = false;
      RollOutOfCloset();
      return;
    }

    float duration = RattleOnOffDurations[RattleOnOffIndex++];

    AsyncTimer.BaseTime.SetTimeout(() => {
      Rattling = !Rattling;
      NextRattleOnOff();
    }, duration);
  }

  void RollOutOfCloset() {
    ClosetSpriteRenderer.sprite = ClosetOpenSprite;
    DoorSpriteRenderer.enabled = true;

    GameObject player = PlayerGameObject.Current;
    ScriptedMovement scriptedMovement = player.GetComponent<ScriptedMovement>();
    Roll roll = player.GetComponent<Roll>();

    roll.StartRolling();

    AsyncTimer.BaseTime.SetTimeout(() => {
      PlayerSpriteGroup.SetActive(true);

      scriptedMovement.FollowPath(
        new List<Vector3>() {
          player.transform.position + (RollDistance * Vector3.right)
        },
        RollSpeed,
        PhaseFinished
      );
    }, 0.1f);
  }
}
