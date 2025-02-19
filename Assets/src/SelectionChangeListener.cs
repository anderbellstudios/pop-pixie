using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionChangeListener : MonoBehaviour {
  public bool SingletonInstance = true;
  private static SelectionChangeListener Current;

  private UnityEvent<GameObject, GameObject> OnSelectionChange = new();
  public GameObject Selected { get; private set; }

  void Awake() {
    if (SingletonInstance) {
#if UNITY_EDITOR
      Debug.Assert(Current == null, "Cannot have multiple singleton SelectionChangeListeners");
#endif
      Current = this;
    }
  }

  public static void AddListener(
    System.Action<GameObject, GameObject> onSelectionChange,
    GameObject bindToGameObject
  ) {
    Current.OnSelectionChange.AddListener((current, previous) => {
      if (bindToGameObject != null) {
        onSelectionChange(current, previous);
      }
    });
  }

  void Update() {
    EventSystem eventSystem = EventSystem.current;
    GameObject currentSelected = eventSystem.currentSelectedGameObject;

    if (currentSelected != Selected) {
      if (currentSelected == null) {
        // Prevent null selection
        eventSystem.SetSelectedGameObject(Selected);
      } else {
        OnSelectionChange.Invoke(currentSelected, Selected);
        Selected = currentSelected;
      }
    }
  }
}
