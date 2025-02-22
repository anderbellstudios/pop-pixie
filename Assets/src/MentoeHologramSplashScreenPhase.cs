using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MentoeHologramSplashScreenPhase : APhase {
  public float Duration;
  public float DelayBeforeCaptionLine;
  public PlayCaptionLine CaptionLine;

  public List<RectTransform> HologramWalls;
  public AnimationCurve WallHeightCurve;
  public float FinalWallHeight;

  public Transform MentoeTransform;
  public AnimationCurve MentoeDriftCurve;
  public float MentoeDriftAmplitude;

  public Image MentoeImage;
  public AnimationCurve MentoeOpacityCurve;

  public Transform TextTransform;
  public AnimationCurve TextDriftCurve;
  public float TextDriftAmplitude;
  public Vector3 TextDriftDirection;

  public Image TextImage;
  public AnimationCurve TextOpacityCurve;

  public Image BackgroundImage;
  public AnimationCurve BackgroundOpacityCurve;

  public GameObject SplashScreenGameObject;

  private Stopwatch AnimationStopwatch;
  private Vector3 MentoeInitialPosition, TextInitialPosition;

  void Start() {
    MentoeInitialPosition = MentoeTransform.localPosition;
    TextInitialPosition = TextTransform.localPosition;

    UpdateWithProgress(0);
  }

  public override void LocalBegin() {
    StateManager.AddState(State.NotPlaying);
    AnimationStopwatch = new Stopwatch.BaseTime();
    AsyncTimer.BaseTime.SetTimeout(PlayCaptionLine, DelayBeforeCaptionLine);
    AsyncTimer.BaseTime.SetTimeout(PhaseFinished, Duration);
  }

  public override void WhilePhaseRunning() {
    UpdateWithProgress(
      TestMode.Enabled ? 0.5f : AnimationStopwatch.Progress(Duration)
    );
  }

  public override void AfterFinished() {
    UpdateWithProgress(1);
    SplashScreenGameObject.SetActive(false);
    StateManager.RemoveState(State.NotPlaying);
  }

  private void UpdateWithProgress(float progress) {
    HologramWalls.ForEach(wall => {
      wall.sizeDelta = new Vector2(
        wall.sizeDelta.x,
        FinalWallHeight * WallHeightCurve.Evaluate(progress)
      );
    });

    MentoeTransform.localPosition = MentoeInitialPosition + (
      Vector3.up * MentoeDriftAmplitude * MentoeDriftCurve.Evaluate(progress)
    );

    MentoeImage.color = new Color(1, 1, 1, MentoeOpacityCurve.Evaluate(progress));

    TextTransform.localPosition = TextInitialPosition + (
      TextDriftDirection.normalized
      * TextDriftAmplitude
      * TextDriftCurve.Evaluate(progress)
    );

    TextImage.color = new Color(1, 1, 1, TextOpacityCurve.Evaluate(progress));

    BackgroundImage.color = new Color(0, 0, 0, BackgroundOpacityCurve.Evaluate(progress));
  }

  private void PlayCaptionLine() {
    CaptionLine.Perform();
  }
}
