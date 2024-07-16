using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneEvents : MonoBehaviour {
  public bool SingletonInstance = true;
  public static SceneEvents Current;

  public ScreenFade Fader;
  public bool ShouldFadeIn = true, ShouldFadeOut = true, WaitForFadeInBeforePermittingExit = false, PauseGameplayDuringFadeOut = false;
  public float FadeInDelay, FadeInDuration, FadeOutDuration, PostFadeOutDelay;
  public bool IsRetry = false;
  public GameObject PopLogoAnimation;

  [SerializeField] public UnityEvent OnFadeIn;

  private bool FadingIn = false, FadingOut = false;
  private string NewSceneName;
  private AsyncOperation PreloadSceneOperation;

  void Awake() {
    if (SingletonInstance)
      Current = this;

    IsRetry = GameOverData.IsRetry;
    GameOverData.IsRetry = false;
  }

  void Start() {
    if (ShouldFadeIn) {
      FadingIn = true;
      Fader.Fade("to black", 0.0f);
      Invoke("FadeIn", FadeInDelay);
    }
  }

  void FadeIn() {
    Fader.Fade("from black", FadeInDuration);
    Invoke("AfterFadeIn", FadeInDuration);
  }

  void AfterFadeIn() {
    FadingIn = false;
    OnFadeIn.Invoke();
  }

  public void ChangeScene(
    string sceneName,
    bool fadeOutMusic = false,
    bool popLogoAnimation = false,
    float overrideFadeOutDuration = -1
  ) {
    if (FadingOut || (WaitForFadeInBeforePermittingExit && FadingIn))
      return;

    if (overrideFadeOutDuration >= 0f) {
      FadeOutDuration = overrideFadeOutDuration;
    }

    NewSceneName = sceneName;

    if (ShouldFadeOut) {
      FadingOut = true;

      if (PauseGameplayDuringFadeOut)
        StateManager.AddState(State.NotPlaying);

      PreloadScene();

      AudioFadeOut.Current.FadeOut(FadeOutDuration, !fadeOutMusic);
      Fader.Fade("to black", FadeOutDuration);
      Invoke("LoadNewScene", FadeOutDuration + PostFadeOutDelay);

      if (popLogoAnimation)
        Instantiate(PopLogoAnimation);
    } else {
      LoadNewScene();
    }
  }

  void PreloadScene() {
    PreloadSceneOperation = SceneManager.LoadSceneAsync(NewSceneName);
    PreloadSceneOperation.allowSceneActivation = false;
  }

  void LoadNewScene() {
    if (PreloadSceneOperation == null) {
      SceneManager.LoadScene(NewSceneName);
    } else {
      PreloadSceneOperation.allowSceneActivation = true;
      PreloadSceneOperation = null;
    }
  }
}
