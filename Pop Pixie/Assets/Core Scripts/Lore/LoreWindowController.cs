using System.Collections;
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
  public GameObject VerticalScrollBar, HorizontalScrollBar;

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
    VerticalScrollBar.GetComponent<Scrollbar>().value = 1f;
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    LoreWindow.SetActive(state);
  }

  void Update() {
    if (WrappedInput.GetButtonDown("Next Weapon")) {
      ZoomLevel += 0.25f;
    } else if (WrappedInput.GetButtonDown("Previous Weapon")) {
      ZoomLevel -= 0.25f;
    }

    ZoomLevel = Mathf.Clamp(ZoomLevel, 1, 4);

    ContentTransform.localScale = new Vector3(ZoomLevel, ZoomLevel, ZoomLevel);

    if (!HorizontalScrollBar.activeSelf)
      EventSystem.current.SetSelectedGameObject(VerticalScrollBar);
  }
}
