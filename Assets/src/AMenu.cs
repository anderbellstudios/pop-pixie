using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class AMenu : MonoBehaviour {
  public bool StartsVisible, StartsInFocus, HideWhenNestedMenuOpen;
  public Vector2Int NavigationSteps = Vector2Int.up;
  public Button FirstSelected = null;
  public GameObject MenuRoot;
  public List<String> CloseMenuControls = new List<String>() { "Cancel" };

  private List<Button> _Buttons = new();

  private bool _Initialized, _Visible, _InFocus, _CloseNextFrame;
  private AMenu _ParentMenu = null;
  private Button LastClickedButton;

  protected List<Button> GetActiveButtons()
    => _Buttons.Where(b => b != null && b.enabled && b.gameObject.activeInHierarchy).ToList();

  void Start() {
    if (RegisterButtonsAutomatically()) {
      Array.ForEach(
        MenuRoot.GetComponentsInChildren<Button>(true),
        RegisterButton
      );
    }

    SetVisible(StartsVisible);
    SetFocus(StartsInFocus);

    LocalStart();

    UpdateNavigation();
    _Initialized = true;
  }

  protected virtual bool RegisterButtonsAutomatically() => true;

  protected void ClearButtons() {
    _Buttons.ForEach(button => Destroy(button.gameObject));
    _Buttons.Clear();
    LastClickedButton = null;
  }

  protected void RegisterButton(Button button) {
    MenuButton mb = button.GetComponent<MenuButton>();
    mb?.SetMenu(this);

    _Buttons.Add(button);

    button.onClick.AddListener(() => {
      MenuSound.current.Play();
      LastClickedButton = button;
    });

    LocalRegisterButton(button);
  }

  protected virtual void LocalRegisterButton(Button button) { }

  protected void UpdateNavigation() {
    if (NavigationSteps.Equals(Vector2Int.zero))
      return;

    List<Button> navigableButtons = GetActiveButtons()
      .Where(b => b.GetComponent<MenuButton>()?.NavigationEnabled ?? false)
      .ToList();

    Func<int, int, Button> getRelativeButton = (int i, int delta) => {
      if (delta == 0)
        return null;
      int j = i + delta;
      if (j < 0)
        return navigableButtons.First();
      if (j >= navigableButtons.Count)
        return navigableButtons.Last();
      return navigableButtons[j];
    };

    for (int i = 0; i < navigableButtons.Count; i++) {
      Button button = navigableButtons[i];

      button.navigation = new Navigation {
        mode = Navigation.Mode.Explicit,
        selectOnUp = getRelativeButton(i, -NavigationSteps.y),
        selectOnDown = getRelativeButton(i, NavigationSteps.y),
        selectOnLeft = getRelativeButton(i, -NavigationSteps.x),
        selectOnRight = getRelativeButton(i, NavigationSteps.x)
      };
    }
  }

  public void ActiveButtonsChanged() {
    if (_Initialized) {
      UpdateNavigation();
    }
  }

  protected virtual void LocalStart() { }

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

  protected virtual void LocalUpdate() { }

  public void Open() => Open(null);

  public void Open(AMenu parentMenu) {
    _ParentMenu = parentMenu;

    SetVisible(true);
    SetFocus(true);

    LocalOpen();
  }

  protected virtual void LocalOpen() { }

  public void OpenNestedMenu(AMenu menu) {
    SetFocus(false);

    if (HideWhenNestedMenuOpen)
      SetVisible(false);

    menu.Open(this);
  }

  public void Close() {
    SetFocus(false);
    SetVisible(false);

    LastClickedButton = null;

    if (_ParentMenu != null) {
      if (_ParentMenu.HideWhenNestedMenuOpen)
        _ParentMenu.SetVisible(true);

      _ParentMenu.SetFocus(true);
    }

    LocalClose();
  }

  protected virtual void LocalClose() { }

  protected void SetVisible(bool visible) {
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

  protected virtual void LocalBecameVisible() { }

  void LostVisibility() {
    EnhancedDataCollection.LogIfEnabled(() => "Menu closed: " + gameObject.name);
    LocalLostVisibility();
  }

  protected virtual void LocalLostVisibility() { }

  protected void SetFocus(bool focus) {
    GetActiveButtons().ForEach(b => b.interactable = focus);
    _InFocus = focus;

    if (focus) {
      GainedFocus();
    } else {
      LostFocus();
    }
  }

  void GainedFocus() {
    Button selectedButton = LastClickedButton ?? FirstSelected ?? GetActiveButtons().FirstOrDefault();
    selectedButton?.Select();
    selectedButton?.OnSelect(null);

    LocalGainedFocus();
  }

  protected virtual void LocalGainedFocus() { }

  void LostFocus() { LocalLostFocus(); }
  protected virtual void LocalLostFocus() { }
}
