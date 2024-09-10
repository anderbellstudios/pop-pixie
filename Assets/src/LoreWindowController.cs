using System;
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
  public RectTransform InnerContentTransform;
  public GameObject LoreWindow;

  private Vector2 Translation;
  private float Scale;

  public void SetTitle(string title) {
    Title.text = title;
  }

  public void SetImage(Sprite image) {
    Image.sprite = image;
    AspectRatioFitter.aspectRatio = image.rect.width / image.rect.height;
  }

  public void Show() {
    Translation = Vector2.zero;
    Scale = 1;
    SetEnabled(true);
    UpdateTransform();
  }

  public void Hide() {
    SetEnabled(false);
  }

  public void Pan(Vector2 delta) {
    Translation -= delta;
    UpdateTransform();
  }

  public void ScreenPan(Vector2 screenDelta) {
    Pan(ScreenVectorToContentVector(screenDelta) * Scale);
  }

  public void CenterOnContentPoint(Vector2 contentPoint) {
    Translation = -contentPoint * Scale;
    UpdateTransform();
  }

  public void CenterOnScreenPoint(Vector2 screenPoint) {
    CenterOnContentPoint(ScreenPointToContentPoint(screenPoint));
  }

  public void ResetPan() {
    CenterOnContentPoint(Vector2.zero);
  }

  public float GetZoom() => Scale;

  public void SetZoomOnScreenPoint(float scale, Vector2 screenPoint) {
    Vector2 contentPoint = ScreenPointToContentPoint(screenPoint);
    Translation -= contentPoint * (scale - Scale);
    Scale = scale;
    UpdateTransform();
  }

  public void SetZoomOnScreenPoint(Func<float, float> scaleFn, Vector2 screenPoint) {
    SetZoomOnScreenPoint(scaleFn(Scale), screenPoint);
  }

  public void SetZoom(float scale) {
    SetZoomOnScreenPoint(scale, CenterOfContentArea);
  }

  public void SetZoom(Func<float, float> scaleFn) {
    SetZoomOnScreenPoint(scaleFn, CenterOfContentArea);
  }

  public Vector2 CenterOfContentArea
    => ContentTransform.parent.TransformPoint(Vector2.zero);

  public void ResetZoom() {
    SetZoom(1);
  }

  public void ResetZoomAndPan() {
    ResetZoom();
    ResetPan();
  }

  public Vector2 ScreenPointToContentPoint(Vector2 screenPoint)
    => ContentTransform.InverseTransformPoint(screenPoint);

  public Vector2 ScreenVectorToContentVector(Vector2 screenVector)
    => ContentTransform.InverseTransformVector(screenVector);

  private void UpdateTransform() {
    ClampTranslation();
    ContentTransform.anchoredPosition = Translation;
    ContentTransform.localScale = new Vector3(Scale, Scale, 1);
  }

  // Do not allow the center of the viewport to leave the content
  private void ClampTranslation() {
    Vector2 scaledContentSize = InnerContentTransform.rect.size * Scale;
    Vector2 maxTranslation = scaledContentSize / 2;
    Translation = Vector2.Max(Translation, -maxTranslation);
    Translation = Vector2.Min(Translation, maxTranslation);
  }

  private void SetEnabled(bool state) {
    LoreWindow.SetActive(state);
  }
}
