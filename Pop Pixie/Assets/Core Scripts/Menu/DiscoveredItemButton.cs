using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoveredItemButton : MonoBehaviour {

  public LoreItem LoreItem;

  public delegate void ClickCallbackDelegate( LoreItem loreItem );
  public ClickCallbackDelegate ClickCallback;

  public void OnClick() {
    ClickCallback( LoreItem );
  }

}
