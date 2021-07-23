using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiscoveredItemsMenuEvents : AMenu {
  public Button BackButton;
  public GameObject ButtonPrefab;
  public Transform ButtonGroup;
  public LoreManager LoreManager;

  public override List<Button> LocalInitButtons() {
    List<Button> buttons = new List<Button>();

    buttons.Add(BackButton);

    foreach (var loreItem in LoreItemData.ReadLoreItems()) {
      GameObject button = Instantiate( ButtonPrefab, ButtonGroup );
      button.transform.Find("TextMeshPro Text").GetComponent<TMP_Text>().text = loreItem.Name;

      var buttonHandler = button.GetComponent<DiscoveredItemButton>();
      buttonHandler.LoreItem = loreItem;
      buttonHandler.ClickCallback = LoreItemButtonClicked;

      buttons.Add(button.GetComponent<Button>());
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
