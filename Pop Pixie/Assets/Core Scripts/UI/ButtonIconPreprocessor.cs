using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class ButtonIconPreprocessor : MonoBehaviour, ITextPreprocessor {
  private string Prefix = "Kb+M";
  private LowPriorityBehaviour LowPriorityBehaviour;

  void OnEnable() {
    LowPriorityBehaviour = new LowPriorityBehaviour();

    TMP_Text text = GetComponent<TMP_Text>();
    text.textPreprocessor = this;
    text.ForceMeshUpdate(true, true);
  }

  void Update() {
    LowPriorityBehaviour.EveryNFrames(10, () => {
      string _prefix = WrappedInput.ControllerPrefix() ?? "Kb+M";

      if (_prefix != Prefix) {
        Prefix = _prefix;
        GetComponent<TMP_Text>().ForceMeshUpdate(true, true);
      }
    });
  }

  public string PreprocessText(string original) {
    if (string.IsNullOrEmpty(original))
      return original;

    return Regex.Replace(original, @"\[([^\[]*)\]", String.Format("<sprite=\"{0} Button Icons\" name=\"$1\">", Prefix));
  }
}
