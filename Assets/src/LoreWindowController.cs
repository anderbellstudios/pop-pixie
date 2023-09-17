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

  public void ViewportPan(Vector2 viewportDelta) {
    Pan(ViewportVectorToContentVector(viewportDelta) * Scale);
  }

  public void CenterOnContentPoint(Vector2 contentPoint) {
    Translation = -contentPoint * Scale;
    UpdateTransform();
  }

  public void ResetPan() {
    CenterOnContentPoint(Vector2.zero);
  }

  public float GetZoom() => Scale;

  public void SetZoomOnViewportPoint(float scale, Vector2 viewportPoint) {
    Vector2 contentPoint = ViewportPointToContentPoint(viewportPoint);
    Translation -= contentPoint * (scale - Scale);
    Scale = scale;
    UpdateTransform();
  }

  public void SetZoomOnViewportPoint(Func<float, float> scaleFn, Vector2 viewportPoint) {
    SetZoomOnViewportPoint(scaleFn(Scale), viewportPoint);
  }

  public void SetZoom(float scale) {
    SetZoomOnViewportPoint(scale, Vector2.zero);
  }

  public void SetZoom(Func<float, float> scaleFn) {
    SetZoomOnViewportPoint(scaleFn, Vector2.zero);
  }

  public void ResetZoom() {
    SetZoom(1);
  }

  public void ResetZoomAndPan() {
    ResetZoom();
    ResetPan();
  }

  // Point

  // Screen -> Content
  public Vector2 ScreenPointToContentPoint(Vector2 screenPoint)
    => ContentTransform.InverseTransformPoint(screenPoint);

  // Screen -> Viewport
  public Vector2 ScreenPointToViewportPoint(Vector2 screenPoint)
    => ContentTransform.parent.InverseTransformPoint(screenPoint);

  // Viewport -> Screen
  public Vector2 ViewportPointToScreenPoint(Vector2 viewportPoint)
    => ContentTransform.parent.TransformPoint(viewportPoint);

  // Viewport -> Content
  public Vector2 ViewportPointToContentPoint(Vector2 viewportPoint)
    => ScreenPointToContentPoint(ViewportPointToScreenPoint(viewportPoint));

  // Vector

  // Screen -> Content
  public Vector2 ScreenVectorToContentVector(Vector2 screenVector)
    => ContentTransform.InverseTransformVector(screenVector);

  // Viewport -> Screen
  public Vector2 ViewportVectorToScreenVector(Vector2 viewportVector)
    => ContentTransform.parent.TransformVector(viewportVector);

  // Viewport -> Content
  public Vector2 ViewportVectorToContentVector(Vector2 viewportVector)
    => ScreenVectorToContentVector(ViewportVectorToScreenVector(viewportVector));

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
