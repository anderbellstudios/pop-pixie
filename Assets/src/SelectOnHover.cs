using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHover
  : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler {
  public Button OverrideButton;

  private Button Button;
  private bool BeforeFirstFrame = true,
    FirstFrame = true;
  private bool Hovered = false;
  private bool SelectOnMouseMove = false;
  private Vector3 PreviousMousePosition;

  void Awake() {
    Button = OverrideButton ?? GetComponent<Button>();
  }

  void Update() {
    if (BeforeFirstFrame) {
      BeforeFirstFrame = false;
    } else if (FirstFrame) {
      FirstFrame = false;
    }

    if (Hovered && SelectOnMouseMove && WrappedInput.MousePosition != PreviousMousePosition) {
      Select();
    }
  }

  void OnEnable() {
    BeforeFirstFrame = true;
    FirstFrame = true;
    Hovered = false;
    SelectOnMouseMove = false;
  }

  public void OnPointerEnter(PointerEventData eventData) {
    Hovered = true;

    if (FirstFrame) {
      PreviousMousePosition = WrappedInput.MousePosition;
      SelectOnMouseMove = true;
    } else {
      Select();
    }
  }

  public void OnPointerExit(PointerEventData eventData) {
    Hovered = false;
    SelectOnMouseMove = false;
  }

  private void Select() {
    SelectOnMouseMove = false;

    if (Button.interactable) {
      EventSystem.current.SetSelectedGameObject(Button.gameObject);
    }
  }

  public void OnSelect(BaseEventData eventData) {
    SelectOnMouseMove = false;
  }

  public void OnDeselect(BaseEventData eventData) {
    if (Hovered) {
      PreviousMousePosition = WrappedInput.MousePosition;
      SelectOnMouseMove = true;
    }
  }
}
