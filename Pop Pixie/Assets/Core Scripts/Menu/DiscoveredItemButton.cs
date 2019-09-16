using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoveredItemButton : MonoBehaviour {

  public LoreItem LoreItem;
  public Button Button;

  public delegate void ClickCallbackDelegate( Button button, LoreItem loreItem );
  public ClickCallbackDelegate ClickCallback;

  public void OnClick() {
    ClickCallback( Button, LoreItem );
  }

}
