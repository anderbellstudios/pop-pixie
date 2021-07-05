using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DiscoveredItemsMenuEvents : MonoBehaviour {

  public static PauseMenuEvents ParentMenu; // Must be set by object that calls LoadScene

  public GameObject ButtonPrefab;
  public Transform ButtonGroup;
  public LoreManager LoreManager;

  bool InFocus;
  List<Button> Buttons;
  Button LastSelectedButton;

  void Start() {
    InFocus = true;

    Buttons = new List<Button>();

    foreach ( var loreItem in LoreItemData.ReadLoreItems() ) {
      GameObject button = Instantiate( ButtonPrefab, ButtonGroup );
      button.transform.Find("Label").GetComponent<TMP_Text>().text = loreItem.Name;

      var buttonHandler = button.GetComponent<DiscoveredItemButton>();
      buttonHandler.LoreItem = loreItem;
      buttonHandler.ClickCallback = LoreItemButtonClicked;

      Buttons.Add( button.GetComponent<Button>() );
    }
  }

  void Update() {
    if ( InFocus && WrappedInput.GetButtonDown("Cancel") ) {
      ResumeParent();
    }
  }

  public void ResumeParent() {
    SceneManager.UnloadSceneAsync("Discovered Items Menu");
    ParentMenu.Focus();
  }

  public void LoreItemButtonClicked( Button button, LoreItem loreItem ) {
    InFocus = false;
    LastSelectedButton = button;

    SetButtonsEnabled(false);

    LoreManager.Open(loreItem, () => {
      SetButtonsEnabled(true);
      LastSelectedButton.Select();
      LastSelectedButton.OnSelect(null);
      InFocus = true;
    });
  }

  void SetButtonsEnabled( bool enabled ) {
    Buttons.ForEach( button => button.interactable = enabled );
  }

}
