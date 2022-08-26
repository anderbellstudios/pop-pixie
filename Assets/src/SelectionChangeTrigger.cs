using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionChangeTrigger : MonoBehaviour {
  public UnityEvent<GameObject, GameObject> OnSelectionChange;

  private GameObject PreviousSelected;

  void Update() {
    GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

    if (currentSelected != PreviousSelected) {
      OnSelectionChange.Invoke(currentSelected, PreviousSelected);
      PreviousSelected = currentSelected;
    }
  }
}
