using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class AMenu : MonoBehaviour {
  public bool StartsVisible, StartsInFocus, HideWhenNestedMenuOpen;
  public Button FirstSelected = null;
  public GameObject MenuRoot;
  public List<String> CloseMenuControls = new List<String>() { "Cancel" };

  private List<Button> _Buttons;
  protected List<Button> Buttons
    => _Buttons.Where(b => b != null).ToList();

  private bool _Visible, _InFocus, _CloseNextFrame;

  private AMenu _ParentMenu = null;

  protected Button LastClickedButton;

  void Start() {
    _Buttons = LocalInitButtons();

    LastClickedButton = FirstSelected ?? Buttons.FirstOrDefault();

    Buttons.ForEach(button =>
      button.onClick.AddListener(() => {
        MenuSound.current.Play();
        LastClickedButton = button;
      })
    );

    LocalStartBeforeSelect();

    SetVisible(StartsVisible);
    SetFocus(StartsInFocus);

    LocalStart();
  }

  public virtual List<Button> LocalInitButtons() {
    return MenuRoot.GetComponentsInChildren<Button>(true).ToList();
  }

  public virtual void LocalStartBeforeSelect() {} 
  public virtual void LocalStart() {} 

  void Update() {
    if (_InFocus) {
      LocalUpdate();

      if (_CloseNextFrame) {
        _CloseNextFrame = false;
        Close();
      }

      if (CloseMenuControls.Any(control => WrappedInput.GetButtonDown(control))) {
        _CloseNextFrame = true;
      }
    }
  }

  public virtual void LocalUpdate() {} 

  public void Open() => Open(null);

  public void Open(AMenu parentMenu) {
    _ParentMenu = parentMenu;

    SetVisible(true);
    SetFocus(true);

    LocalOpen();
  }

  public virtual void LocalOpen() {}

  public void OpenNestedMenu(AMenu menu) {
    SetFocus(false);

    if (HideWhenNestedMenuOpen)
      SetVisible(false);

    menu.Open(this);
  }

  public void Close() {
    SetFocus(false);
    SetVisible(false);

    if (_ParentMenu != null) {
      if (_ParentMenu.HideWhenNestedMenuOpen)
        _ParentMenu.SetVisible(true);

      _ParentMenu.SetFocus(true);
    }

    LocalClose();

  }

  public virtual void LocalClose() {}

  public void SetVisible(bool visible) {
    MenuRoot.SetActive(visible);

    bool wasVisible = _Visible;
    _Visible = visible;

    if (visible && !wasVisible) {
      BecameVisible();
    } else if (!visible && wasVisible) {
      LostVisibility();
    }
  }

  void BecameVisible() {
    EnhancedDataCollection.LogIfEnabled(() => "Menu opened " + gameObject.name);
    LocalBecameVisible();
  }

  public virtual void LocalBecameVisible() {}

  void LostVisibility() {
    EnhancedDataCollection.LogIfEnabled(() => "Menu closed: " + gameObject.name);
    LocalLostVisibility();
  }

  public virtual void LocalLostVisibility() {}

  public void SetFocus(bool focus) {
    Buttons.ForEach(b => b.interactable = focus);
    _InFocus = focus;

    if (focus) {
      GainedFocus();
    } else {
      LostFocus();
    }
  }

  void GainedFocus() {
    LastClickedButton?.Select();
    LastClickedButton?.OnSelect(null);

    LocalGainedFocus();
  }

  public virtual void LocalGainedFocus() {}

  void LostFocus() { LocalLostFocus(); }
  public virtual void LocalLostFocus() {}
}
