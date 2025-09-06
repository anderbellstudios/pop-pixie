using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WizardModeChangeSceneButton : MonoBehaviour {
  public Button Button;
  public TMP_Text Label;

  public UnityEvent OnClick;

  public void SetSceneName(string sceneName) {
    Label.text = sceneName;
  }

  public void HandleClick() {
    OnClick.Invoke();
  }
}
