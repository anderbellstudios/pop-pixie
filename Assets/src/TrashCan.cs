using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : AInspectable {
  public DialogueHopper FirstDialogue, SecondDialogue, FinalDialogue;
  public ParticleSystem ParticleSystem;
  public Transform PopPixieSpriteTransform, PivotTransform;
  public SpriteRenderer PopPixieSpriteRenderer;
  public SpawnFlyingRingPull SpawnFlyingRingPull;
  public SpriteRenderer TrashCanFrontSprite, TrashCanBackSprite;
  public Sprite TrashCanFrontSpriteEmpty, TrashCanBackSpriteEmpty;

  public float ZenithOffsetY, FinalOffsetX, FinalOffsetY;
  public float JumpDuration;
  public float TrashCanWobbleSpeed, TrashCanWobbleAmplitude, PopPixieWobbleSpeed, PopPixieWobbleAmplitude;
  public float DigDuration;
  public SoundHopper DigSound;

  enum JumpDirectionEnum { In, Out };

  private int Stage;

  private bool Jumping = false;
  private AnimationCurve JumpXCurve, JumpYCurve, JumpRotationCurve;
  private IntervalTimer JumpTimer;
  private JumpDirectionEnum JumpDirection;

  private bool Digging = false;
  private IntervalTimer DigTimer;

  void Awake() {
    Stage = 0;

    JumpTimer = new IntervalTimer() {
      Interval = JumpDuration
    };

    JumpXCurve = new AnimationCurve();
    JumpYCurve = new AnimationCurve();
    JumpRotationCurve = new AnimationCurve();

    DigTimer = new IntervalTimer() {
      Interval = DigDuration
    };
  }

  void Start() {
    AInspectableStart();
  }

  public override String AInspectablePromptText() {
    return "Press [Inspect] to inspect";
  }

  public override void OnInspect() {
    switch (Stage) {
      case 0:
        FirstDialogue.Hop();
        break;

      case 1:
        Stage++;
        SecondDialogue.Hop();
        break;

      default:
        Stage++;
        FinalDialogue.Hop();
        break;
    }
  }

  public void BeginAnimation() {
    Stage++;

    Vector3 playerPosition = PlayerGameObject.Current.transform.position;

    PopPixieSpriteTransform.position = playerPosition;

    JumpXCurve.AddKey(0, playerPosition.x);
    JumpYCurve.AddKey(0, playerPosition.y);
    JumpRotationCurve.AddKey(0, 0);

    JumpYCurve.AddKey(0.5f, transform.position.y + ZenithOffsetY);

    JumpXCurve.AddKey(1, transform.position.x + FinalOffsetX);
    JumpYCurve.AddKey(1, transform.position.y + FinalOffsetY);
    JumpRotationCurve.AddKey(1, -180);

    SetCutscene(true);

    BeginJumping(JumpDirectionEnum.In);
  }

  void BeginJumping(JumpDirectionEnum jumpDirection) {
    JumpTimer.Reset();
    JumpDirection = jumpDirection;
    Jumping = true;
  }

  void EndJumping() {
    Jumping = false;
  }

  void BeginDigging() {
    DigTimer.Reset();
    Digging = true;
    DigSound.Hop();
  }

  void EndDigging() {
    Digging = false;
    SetTrashCanAngle(0);
  }

  void SetCutscene(bool inControl) {
    PlayerGameObject.Current.transform.localScale = inControl ? Vector3.zero : Vector3.one;
    PopPixieSpriteRenderer.enabled = inControl;

    if (inControl) {
      StateManager.AddState(State.NotPlaying);
    } else {
      StateManager.RemoveState(State.NotPlaying);
    }
  }

  void SetTrashCanAngle(float angle) {
    PivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
  }

  void SetPopPixieAngle(float angle) {
    PopPixieSpriteTransform.localRotation = Quaternion.Euler(0, 0, angle);
  }

  void Update() {
    AInspectableUpdate();

    if (Jumping) {
      HandleJumping();
    }

    if (Digging) {
      HandleDigging();
    }
  }

  void HandleJumping() {
    float progress = JumpDirection == JumpDirectionEnum.In
      ? JumpTimer.Progress()
      : 1 - JumpTimer.Progress();

    PopPixieSpriteRenderer.sortingLayerName = progress < 0.5 ? "Character" : "Level elements";

    PopPixieSpriteTransform.position = new Vector3(
      JumpXCurve.Evaluate(progress),
      JumpYCurve.Evaluate(progress),
      0
    );

    PopPixieSpriteTransform.localRotation = Quaternion.Euler(0, 0, JumpRotationCurve.Evaluate(progress));

    JumpTimer.IfElapsed(() => {
      EndJumping();

      if (JumpDirection == JumpDirectionEnum.In) {
        ParticleSystem.Play();
        BeginDigging();
      } else {
        SetCutscene(false);
      }
    });
  }

  void HandleDigging() {
    SetTrashCanAngle(TrashCanWobbleAmplitude * Mathf.Sin(TrashCanWobbleSpeed * Time.time));
    SetPopPixieAngle(180 + PopPixieWobbleAmplitude * Mathf.Sin(PopPixieWobbleSpeed * Time.time));

    DigTimer.IfElapsed(() => {
      EndDigging();
      SpawnFlyingRingPull.Instantiate();
      TrashCanFrontSprite.sprite = TrashCanFrontSpriteEmpty;
      TrashCanBackSprite.sprite = TrashCanBackSpriteEmpty;
      BeginJumping(JumpDirectionEnum.Out);
    });
  }
}
