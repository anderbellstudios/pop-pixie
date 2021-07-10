using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGamePrompt : MonoBehaviour {
  public delegate String InGamePromptSource();

  public static InGamePrompt Current;

  public TMP_Text Text;

  private List<InGamePromptSource> Sources = new List<InGamePromptSource>();

  void Awake() {
    Current = this;
  }

  public void RegisterSource(InGamePromptSource source) {
    Sources.Add(source);
  }

  void Update() {
    if (StateManager.Isnt(State.Playing))
      return;

    Text.text = CurrentText();
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
