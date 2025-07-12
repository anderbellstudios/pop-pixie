using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscoveredItemsMenuEvents : AMenu {
  public RegisteredLoreItems RegisteredLoreItems;
  public GameObject MenuItemPrefab;
  public Transform MenuItemContainer;
  public RectTransform ScrollContentArea;
  public ScrollRect ScrollRect;
  public LoreManager LoreManager;
  public bool Debug = false;

  protected override void LocalStart() {
    foreach (Transform child in MenuItemContainer) {
      Destroy(child.gameObject);
    }

    List<LoreItem> loreItems = Debug
      ? RegisteredLoreItems.LoreItems
      : LoreItemData.ReadLoreItems().Select(id => RegisteredLoreItems.Find(id)).ToList();

    foreach (LoreItem loreItem in loreItems) {
      GameObject menuItemGameObject = Instantiate(MenuItemPrefab, MenuItemContainer);

      DiscoveredItemButton discoveredItemButton =
        menuItemGameObject.GetComponent<DiscoveredItemButton>();

      discoveredItemButton.SetLoreItem(loreItem);
      discoveredItemButton.ClickCallback = LoreItemButtonClicked;

      discoveredItemButton.SelectCallback = () =>
        ScrollToSelectionHelper.EnsureVisible(
          targetTransform: (RectTransform)menuItemGameObject.transform,
          contentArea: ScrollContentArea,
          scrollRect: ScrollRect
        );

      RegisterButton(discoveredItemButton.Button);
    }
  }

  private void LoreItemButtonClicked(Button button, LoreItem loreItem) {
    SetFocus(false);
    SetVisible(false);

    LoreManager.Open(
      loreItem,
      () => {
        SetVisible(true);
        SetFocus(true);
      }
    );
  }
}
