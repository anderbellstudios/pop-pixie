using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuSound : MonoBehaviour {
  public static MenuSound Current
    => EventSystem.current.gameObject.GetComponent<MenuSound>();

  public UnityEvent OnPlay;

  void Start() {
    SelectionChangeListener.AddListener(HandleSelectionChange, gameObject);
  }

  private void HandleSelectionChange(GameObject current, GameObject previous) {
    if (current == null || previous == null)
      return;

    MenuButton currentMenuButton = current.GetComponent<MenuButton>();
    MenuButton previousMenuButton = previous.GetComponent<MenuButton>();

    if (currentMenuButton == null || previousMenuButton == null)
      return;

    if (
      currentMenuButton.MenuSoundEnabled &&
      currentMenuButton.Menu == previousMenuButton.Menu
    )
      Play();
  }

  public void Play() {
    OnPlay.Invoke();
  }
}
