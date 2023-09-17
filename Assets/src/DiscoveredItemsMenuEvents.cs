using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiscoveredItemsMenuEvents : AMenu {
  public RegisteredLoreItems RegisteredLoreItems;
  public GameObject MenuItemPrefab;
  public Transform MenuItemContainer;
  public RectTransform ScrollContentArea;
  public ScrollRect ScrollRect;
  public LoreManager LoreManager;

  public override List<Button> LocalInitButtons() {
    List<Button> buttons = new List<Button>();

    foreach (Transform child in MenuItemContainer) {
      Destroy(child.gameObject);
    }

    foreach (string loreItemId in LoreItemData.ReadLoreItems()) {
      LoreItem loreItem = RegisteredLoreItems.Find(loreItemId);

      GameObject menuItemGameObject = Instantiate(MenuItemPrefab, MenuItemContainer);

      DiscoveredItemButton discoveredItemButton = menuItemGameObject.GetComponent<DiscoveredItemButton>();

      discoveredItemButton.SetLoreItem(loreItem);
      discoveredItemButton.ClickCallback = LoreItemButtonClicked;

      discoveredItemButton.SelectCallback = () => ScrollToSelectionHelper.EnsureVisible(
        targetTransform: (RectTransform)menuItemGameObject.transform,
        contentArea: ScrollContentArea,
        scrollRect: ScrollRect
      );

      buttons.Add(discoveredItemButton.Button);
    }

    return buttons;
  }

  public void LoreItemButtonClicked(Button button, LoreItem loreItem) {
    SetFocus(false);
    SetVisible(false);

    LoreManager.Open(loreItem, () => {
      SetVisible(true);
      SetFocus(true);
    });
  }
}
