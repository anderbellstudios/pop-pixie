using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MaxAspectRatio : MonoBehaviour {
  private const float AspectRatio = 2f;

  public RectTransform Parent;

  void Start() {
    Self.anchorMin = new Vector2(0.5f, 0f);
    Self.anchorMax = new Vector2(0.5f, 1f);
  }

  void Update() {
    float maxWidth = Parent.rect.height * AspectRatio;
    float width = Mathf.Min(maxWidth, Parent.rect.width);
    Self.sizeDelta = new Vector2(width, 0f);
  }

  private RectTransform Self => transform as RectTransform;
}
