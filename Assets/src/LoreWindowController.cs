﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LoreWindowController : MonoBehaviour {

  public TMP_Text Title;
  public Image Image;
  public AspectRatioFitter AspectRatioFitter;
  public RectTransform ContentTransform;
  public GameObject LoreWindow;
  public GameObject VerticalScrollBar;
  public TMP_Text ZoomControlText;

  private float ZoomLevel;

  public void SetTitle(string title) {
    Title.text = title;
  }

  public void SetImage(Sprite image) {
    Image.sprite = image;
    AspectRatioFitter.aspectRatio = image.rect.width / image.rect.height;
  }

  public void Show () {
    ZoomLevel = 1;
    SetEnabled(true);
    ZoomLevelWasUpdated();
    VerticalScrollBar.GetComponent<Scrollbar>().value = 1f;
    EventSystem.current.SetSelectedGameObject(VerticalScrollBar);
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    LoreWindow.SetActive(state);
  }

  void Update() {
    if (WrappedInput.GetButtonDown("Next Weapon")) {
      ChangeZoomLevel(0.25f);
    } else if (WrappedInput.GetButtonDown("Previous Weapon")) {
      ChangeZoomLevel(-0.25f);
    }
  }

  void ChangeZoomLevel(float delta) {
    ZoomLevel = Mathf.Clamp(ZoomLevel + delta, 1, 4);
    ZoomLevelWasUpdated();
  }

  void ZoomLevelWasUpdated() {
    ContentTransform.localScale = new Vector3(ZoomLevel, ZoomLevel, ZoomLevel);

    ZoomControlText.text = ZoomLevel > 1
      ? "Zoom out <size=150%>[Previous Weapon]</size>"
      : "Zoom in <size=150%>[Next Weapon]</size>";
  }
}