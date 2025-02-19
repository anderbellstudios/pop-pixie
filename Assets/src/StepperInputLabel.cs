using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StepperInputLabel : MonoBehaviour, IPointerClickHandler {
  public Button Button;

  public void OnPointerClick(PointerEventData eventData) {
    Button.onClick.Invoke();
  }
}
