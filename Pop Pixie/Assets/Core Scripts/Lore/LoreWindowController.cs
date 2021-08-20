using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LoreWindowController : MonoBehaviour {

  public TMP_Text Title;
  public PercentageButton ZoomLevelButton;
  public Image Image;
  public AspectRatioFitter AspectRatioFitter;
  public RectTransform ContentTransform;
  public GameObject LoreWindow;
  public GameObject ScrollBar;

  void Start() {
    decimal zoomLevel = OptionsData.LoreWindowZoomLevel;

    ZoomLevelButton.Value = zoomLevel;
    ZoomLevelButton.MaxValue = 1M;
    ZoomLevelButton.MinValue = 0.05M;
    ZoomLevelButton.Increment = 0.05M;
    ZoomLevelButton.UpdateValue();

    SetRelativeWidth((float) zoomLevel);
  }

  public void SetZoomLevel(decimal zoomLevel) {
    OptionsData.LoreWindowZoomLevel = zoomLevel;
    SetRelativeWidth((float) zoomLevel);
  }

  public void SetTitle(string title) {
    Title.text = title;
  }

  public void SetImage(Sprite image) {
    Image.sprite = image;
    AspectRatioFitter.aspectRatio = image.rect.width / image.rect.height;
  }

  public void SetRelativeWidth(float relativeWidth) {
    float horizontalPadding = ((RectTransform) ContentTransform.parent).rect.width * (1f - relativeWidth) / 2f;
    ContentTransform.offsetMin = new Vector2(horizontalPadding, 0);
    ContentTransform.offsetMax = new Vector2(-horizontalPadding, 0);
  }

  public void Show () {
    SetEnabled(true);
    ScrollBar.GetComponent<Scrollbar>().value = 1f;
    EventSystem.current.SetSelectedGameObject( ScrollBar );
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    LoreWindow.SetActive(state);
  }
}
