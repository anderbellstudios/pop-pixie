using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiscoveredItemsMenuEvents : AMenu {
  public RegisteredLoreItems RegisteredLoreItems;
  public Button BackButton;
  public GameObject MenuItemPrefab;
  public Transform MenuItemContainer;
  public RectTransform ScrollContentArea;
  public ScrollRect ScrollRect;
  public LoreManager LoreManager;

  public override List<Button> LocalInitButtons() {
    List<Button> buttons = new List<Button>();

    buttons.Add(BackButton);

    foreach (Transform child in MenuItemContainer) {
      Destroy(child.gameObject);
    }

    foreach (string loreItemId in LoreItemData.ReadLoreItems()) {
      LoreItem loreItem = RegisteredLoreItems.Find(loreItemId);

      GameObject menuItemGameObject = Instantiate(MenuItemPrefab, MenuItemContainer);

      DiscoveredItemButton discoveredItemButton = menuItemGameObject.GetComponent<DiscoveredItemButton>();

      discoveredItemButton.SetLoreItem(loreItem);
      discoveredItemButton.ClickCallback = LoreItemButtonClicked;

      discoveredItemButton.SelectCallback = () => {
        Canvas.ForceUpdateCanvases();

        RectTransform targetTransform = (RectTransform) menuItemGameObject.transform;

        float targetPositionY =
          ScrollRect.transform.InverseTransformPoint(ScrollContentArea.position).y
          - ScrollRect.transform.InverseTransformPoint(targetTransform.position).y;

        float targetHeight = targetTransform.sizeDelta.y;
        float targetTopEdge = targetPositionY - (targetHeight / 2f);
        float targetBottomEdge = targetPositionY + (targetHeight / 2f);

        float viewportTopEdge = ScrollContentArea.anchoredPosition.y;
        float viewportBottomEdge = viewportTopEdge + ((RectTransform) ScrollRect.transform).rect.height;

        if (targetBottomEdge > viewportBottomEdge) {
          ScrollContentArea.anchoredPosition =
            ScrollContentArea.anchoredPosition
            + new Vector2(
                0,
                targetBottomEdge - viewportBottomEdge
              );
        }

        if (targetTopEdge < viewportTopEdge) {
          ScrollContentArea.anchoredPosition =
            ScrollContentArea.anchoredPosition
            - new Vector2(
                0,
                viewportTopEdge - targetTopEdge
              );
        }
      };

      buttons.Add(discoveredItemButton.Button);
    }

    return buttons;
  }

  public void BackButtonClicked() {
    Close();
  }

  public void LoreItemButtonClicked( Button button, LoreItem loreItem ) {
    SetFocus(false);
    SetVisible(false);

    LoreManager.Open(loreItem, () => {
      SetVisible(true);
      SetFocus(true);
    });
  }
}
