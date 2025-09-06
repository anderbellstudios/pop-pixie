using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Scaler : MonoBehaviour {
  public RectTransform Parent;
  public float MaxRelativeWidth = 1f;
  public float MaxRelativeHeight = 1f;

  void Update() {
    Self.localScale = Vector3.one * Scale();
  }

  private float Scale() => Mathf.Min(WidthScale(), HeightScale());

  private float WidthScale() =>
    GenericScale(
      ownSize: Self.sizeDelta.x,
      parentSize: Parent.sizeDelta.x,
      maxRelativeSize: MaxRelativeWidth
    );

  private float HeightScale() =>
    GenericScale(
      ownSize: Self.sizeDelta.y,
      parentSize: Parent.sizeDelta.y,
      maxRelativeSize: MaxRelativeHeight
    );

  private float GenericScale(float ownSize, float parentSize, float maxRelativeSize) =>
    Mathf.Min(1f, maxRelativeSize * parentSize / ownSize);

  private RectTransform Self => transform as RectTransform;
}
