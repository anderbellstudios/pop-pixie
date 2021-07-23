using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DiscoveredItemButton : MonoBehaviour, ISelectHandler {
  public LoreItem LoreItem;
  public Button Button;
  public TMP_Text Label;

  public delegate void ClickCallbackDelegate( Button button, LoreItem loreItem );
  public ClickCallbackDelegate ClickCallback;
  public Action SelectCallback;

  public void SetLoreItem(LoreItem loreItem) {
    LoreItem = loreItem;
    Label.text = loreItem.Name;
  }

  public void OnClick() {
    ClickCallback( Button, LoreItem );
  }

  public void OnSelect(BaseEventData eventData) {
    if (SelectCallback != null)
      SelectCallback();
  }
}
