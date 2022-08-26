using UnityEngine;
using UnityEngine.UI;

public static class ScrollToSelectionHelper {
  public static void EnsureVisible(RectTransform targetTransform, RectTransform contentArea, ScrollRect scrollRect) {
    Canvas.ForceUpdateCanvases();

    float targetPositionY =
      scrollRect.transform.InverseTransformPoint(contentArea.position).y
      - scrollRect.transform.InverseTransformPoint(targetTransform.position).y;

    float targetHeight = targetTransform.sizeDelta.y;
    float targetTopEdge = targetPositionY - (targetHeight / 2f);
    float targetBottomEdge = targetPositionY + (targetHeight / 2f);

    float viewportTopEdge = contentArea.anchoredPosition.y;
    float viewportBottomEdge = viewportTopEdge + ((RectTransform) scrollRect.transform).rect.height;

    if (targetBottomEdge > viewportBottomEdge) {
      contentArea.anchoredPosition += new Vector2(
        0,
        targetBottomEdge - viewportBottomEdge
      );
    } else if (targetTopEdge < viewportTopEdge) {
      contentArea.anchoredPosition -= new Vector2(
        0,
        viewportTopEdge - targetTopEdge
      );
    }
  }
}
