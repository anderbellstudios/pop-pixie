using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StepperInputLabel : MonoBehaviour, IPointerClickHandler {
  public Button Button;

  public void OnPointerClick(PointerEventData eventData) {
    Button.onClick.Invoke();
  }
}
