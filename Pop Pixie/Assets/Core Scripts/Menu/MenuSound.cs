using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSound : MonoBehaviour {
  public static MenuSound current
    => EventSystem.current.gameObject.GetComponent<MenuSound>();

  public EventSystem EventSystem;
  public SoundHopper SoundHopper;

  private GameObject PreviousSelected;

  void Update() {
    GameObject currentSelected = EventSystem.currentSelectedGameObject;

    if (currentSelected != PreviousSelected) {
      String currentSelectedMenuName = MenuNameForGameObject(currentSelected);
      String previousSelectedMenuName = MenuNameForGameObject(PreviousSelected);

#if UNITY_EDITOR
      Debug.Assert(currentSelectedMenuName != "", "Menu name cannot be empty");
#endif

      if (currentSelectedMenuName == previousSelectedMenuName && currentSelectedMenuName != null)
        Play();

      PreviousSelected = currentSelected;
    }
  }

  public void Play() {
    SoundHopper.Hop();
  }

  private String MenuNameForGameObject(GameObject go) {
    if (go == null)
      return null;

    return go.GetComponent<ShouldPlayMenuSound>()?.MenuName;
  }
}
