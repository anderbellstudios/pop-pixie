using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StepperInput : MonoBehaviour {
  public TMP_Text Text;
  public Button Button;

  public List<string> Options;
  public int Value = 0;
  public string ValueLabel => Options[Value];

  public UnityEvent<int, string> OnChange;

  private bool Selected = false;

  void Awake() {
    UpdateLabel();
  }

  public void OnSelect(BaseEventData eventData) {
    Selected = true;
  }

  public void OnDeselect(BaseEventData eventData) {
    Selected = false;
  }

  void Update() {
    if (Selected) {
      if (WrappedInput.GetButtonDown("Right")) {
        MenuSound.current.Play();
        IncrementValue();
      } else if (WrappedInput.GetButtonDown("Left")) {
        MenuSound.current.Play();
        DecrementValue();
      }
    }
  }

  public void IncrementValue() {
    Value++;

    if (Value == Options.Count)
      Value = 0;

    ValueChanged();
  }

  public void DecrementValue() {
    Value--;

    if (Value == -1)
      Value = Options.Count - 1;

    ValueChanged();
  }

  public void ValueChanged() {
    UpdateLabel();
    OnChange.Invoke(Value, ValueLabel);

    // If arrows were clicked, selection must be restored
    Button.Select();
  }

  public void UpdateLabel() {
    Text.text = ValueLabel;
  }

  public int ValueForLabel(string label)
    => Options.IndexOf(label);
}
