using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class ValueChangedEvent : UnityEvent<decimal> {}

public class PercentageButton : MonoBehaviour {

  public decimal Value = 1M;
  public TMP_Text Text;

  public ValueChangedEvent OnValueChanged;

  public void Pressed() {
    Value -= 0.1M;

    if ( Value < 0M )
      Value = 1M;

    UpdateValue();

    OnValueChanged.Invoke(Value);
  }

  public void UpdateValue() {
    Text.text = Regex.Replace(
      Text.text,
      @"\[ .* \]",
      "[ " + Value.ToString("P0") + " ]"
    );
  }

}
