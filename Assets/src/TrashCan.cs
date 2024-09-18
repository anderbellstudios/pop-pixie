using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
  public UnityEvent OnBeginDigging;

  enum JumpDirectionEnum { In, Out };

  private int Stage;
  private bool Jumping = false;
  private bool Digging = false;
  private AnimationCurve JumpXCurve, JumpYCurve, JumpRotationCurve;
  private Stopwatch JumpStopwatch;
  private JumpDirectionEnum JumpDirection;

  void Awake() {
    Stage = 0;
    JumpXCurve = new AnimationCurve();
    JumpYCurve = new AnimationCurve();
    JumpRotationCurve = new AnimationCurve();
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

  private void BeginJumping(JumpDirectionEnum jumpDirection) {
    JumpStopwatch = new Stopwatch.BaseTime();
    JumpDirection = jumpDirection;
    Jumping = true;
  }

  private void EndJumping() {
    Jumping = false;
  }

  private void BeginDigging() {
    Digging = true;
    OnBeginDigging.Invoke();

    AsyncTimer.BaseTime.SetTimeout(() => {
      EndDigging();
      SpawnFlyingRingPull.Instantiate();
      TrashCanFrontSprite.sprite = TrashCanFrontSpriteEmpty;
      TrashCanBackSprite.sprite = TrashCanBackSpriteEmpty;
      BeginJumping(JumpDirectionEnum.Out);
    }, DigDuration);
  }

  private void EndDigging() {
    Digging = false;
    SetTrashCanAngle(0);
  }

  private void SetCutscene(bool inControl) {
    PlayerGameObject.Current.transform.localScale = inControl ? Vector3.zero : Vector3.one;
    PopPixieSpriteRenderer.enabled = inControl;

    if (inControl) {
      StateManager.AddState(State.NotPlaying);
    } else {
      StateManager.RemoveState(State.NotPlaying);
    }
  }

  private void SetTrashCanAngle(float angle) {
    PivotTransform.localRotation = Quaternion.Euler(0, 0, angle);
  }

  private void SetPopPixieAngle(float angle) {
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

  private void HandleJumping() {
    float progress = JumpStopwatch.Progress(JumpDuration);

    float directedProgress = JumpDirection == JumpDirectionEnum.In
      ? progress
      : 1f - progress;

    PopPixieSpriteRenderer.sortingLayerName = directedProgress < 0.5 ? "Character" : "Level elements";

    PopPixieSpriteTransform.position = new Vector3(
      JumpXCurve.Evaluate(directedProgress),
      JumpYCurve.Evaluate(directedProgress),
      0
    );

    PopPixieSpriteTransform.localRotation = Quaternion.Euler(
      0,
      0,
      JumpRotationCurve.Evaluate(directedProgress)
    );

    if (progress >= 1f) {
      EndJumping();

      if (JumpDirection == JumpDirectionEnum.In) {
        ParticleSystem.Play();
        BeginDigging();
      } else {
        SetCutscene(false);
      }
    }
  }

  private void HandleDigging() {
    SetTrashCanAngle(TrashCanWobbleAmplitude * Mathf.Sin(TrashCanWobbleSpeed * Time.time));
    SetPopPixieAngle(180 + PopPixieWobbleAmplitude * Mathf.Sin(PopPixieWobbleSpeed * Time.time));
  }
}
