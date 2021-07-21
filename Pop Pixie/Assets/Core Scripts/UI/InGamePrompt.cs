using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGamePrompt : MonoBehaviour {
  public delegate String InGamePromptSource();

  public bool SingletonInstance = true;
  public static InGamePrompt Current;

  public TMP_Text Text;

  private LowPriorityBehaviour LowPriorityBehaviour;
  private List<InGamePromptSource> Sources = new List<InGamePromptSource>();

  void Awake() {
    if (SingletonInstance)
      Current = this;

    LowPriorityBehaviour = new LowPriorityBehaviour();
  }

  public void RegisterSource(InGamePromptSource source) {
    Sources.Add(source);
  }

  void Update() {
    LowPriorityBehaviour.EveryNFrames(10, () => {
      if (StateManager.Isnt(State.Playing))
        return;

      Text.text = CurrentText();
    });
  }

  String CurrentText() {
    foreach (InGamePromptSource source in Sources) {
      String maybeText = source();

      if (maybeText != null)
        return maybeText;
    }

    return null;
  }
}
