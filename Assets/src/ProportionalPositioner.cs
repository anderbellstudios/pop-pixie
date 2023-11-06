using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProportionalPositioner : MonoBehaviour {
  public RectTransform ModelParent, ModelTarget, Parent, Target;

  void Update() {
    // Model space
    float modelParentWidth = ModelParent.rect.width;
    float modelParentHeight = ModelParent.rect.height;

    float modelTargetWidth = ModelTarget.rect.width;
    float modelTargetHeight = ModelTarget.rect.height;

    Vector2 modelTargetPosition = GetModelTargetPosition();
    float modelTargetX = modelTargetPosition.x;
    float modelTargetY = modelTargetPosition.y;

    // Actual space
    float actualParentWidth = Parent.rect.width;
    float actualParentHeight = Parent.rect.height;

    // Model space -> relative space (0..1)
    float relativeTargetX = modelTargetX / modelParentWidth;
    float relativeTargetY = modelTargetY / modelParentHeight;

    float relativeTargetHeight = modelTargetHeight / modelParentHeight;
    float relativeTargetWidth = modelTargetWidth / modelParentWidth;

    // Relative space -> actual space
    float actualTargetX = relativeTargetX * actualParentWidth;
    float actualTargetY = relativeTargetY * actualParentHeight;

    float actualTargetWidth = relativeTargetWidth * actualParentWidth;
    float actualTargetHeight = relativeTargetHeight * actualParentHeight;

    // Apply position and size
    Target.anchoredPosition = new Vector2(actualTargetX, actualTargetY);
    Target.sizeDelta = new Vector2(actualTargetWidth, actualTargetHeight);
  }

  private Vector2 GetModelTargetPosition() {
    Vector2 parentWorld = GetMinCornerInWorld(ModelParent);
    Vector2 targetWorld = GetMinCornerInWorld(ModelTarget);
    return ModelParent.InverseTransformVector(targetWorld - parentWorld);
  }

  private Vector2 GetMinCornerInWorld(RectTransform rectTransform) {
    Vector2 localMinCorner = rectTransform.rect.min;
    Vector2 worldMinCorner = rectTransform.TransformPoint(localMinCorner);
    return worldMinCorner;
  }
}
