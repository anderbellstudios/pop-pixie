using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using PopPixie.Audio;

public class SceneEvents : MonoBehaviour {

  public bool SingletonInstance = true;
  public static SceneEvents Current;

  public ScreenFade Fader;
  public bool ShouldFadeIn = true, ShouldFadeOut = true, WaitForFadeInBeforePermittingExit = false, PauseGameplayDuringFadeOut = false;
  public float FadeInDelay, FadeInDuration, FadeOutDuration, PostFadeOutDelay;

  [SerializeField] public UnityEvent OnFadeIn;

  private bool FadingIn = false, FadingOut = false;
  private string NewSceneName;

  void Start() {
    if (SingletonInstance)
      Current = this;

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

  public void ChangeScene(string sceneName) {
    if ( FadingOut || (WaitForFadeInBeforePermittingExit && FadingIn) )
      return;

    NewSceneName = sceneName;

    if (ShouldFadeOut) {
      FadingOut = true;

      if (PauseGameplayDuringFadeOut)
        StateManager.SetState( State.LoadingLevel );

      AudioMixer.Current.FadeOut( FadeOutDuration );
      Fader.Fade("to black", FadeOutDuration);
      Invoke("LoadNewScene", FadeOutDuration + PostFadeOutDelay);
    } else {
      LoadNewScene();
    }
  }

  void LoadNewScene() {
    if (PauseGameplayDuringFadeOut)
      StateManager.SetState( State.Playing );

    SceneManager.LoadScene(NewSceneName);
  }
}
