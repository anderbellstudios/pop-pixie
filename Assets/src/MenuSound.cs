using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuSound : MonoBehaviour {
  public static MenuSound current
    => EventSystem.current.gameObject.GetComponent<MenuSound>();

  public UnityEvent OnPlay;

  public void HandleSelectionChanged(GameObject currentSelected, GameObject previousSelected) {
    String currentSelectedMenuName = MenuNameForGameObject(currentSelected);
    String previousSelectedMenuName = MenuNameForGameObject(previousSelected);

#if UNITY_EDITOR
    Debug.Assert(currentSelectedMenuName != "", "Menu name cannot be empty");
#endif

    if (currentSelectedMenuName == previousSelectedMenuName && currentSelectedMenuName != null)
      Play();
  }

  public void Play() {
    OnPlay.Invoke();
  }

  private String MenuNameForGameObject(GameObject go) {
    if (go == null)
      return null;

    return go.GetComponent<ShouldPlayMenuSound>()?.MenuName;
  }
}
