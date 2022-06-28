using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MutuallyExclusiveHighlight : MonoBehaviour, IPointerEnterHandler, IDeselectHandler {
  public void OnPointerEnter(PointerEventData eventData) {
    var button = Button();

    if ( button.interactable && InputMode.ReceivedInput && InputMode.IsMouseAndKeyboard() ) {
      button.Select();
      button.OnSelect(null);
    }
  }

  public void OnDeselect(BaseEventData eventData) {
    // Create and trigger an artificial OnPointerExit event to turn off highlighting
    Button().OnPointerExit( eventData as PointerEventData );
  }

  Button Button() {
    return GetComponent<Button>();
  }

}
