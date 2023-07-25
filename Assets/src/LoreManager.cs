using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour {
  public delegate void LoreWindowOnClose();

  public bool SingletonInstance = true;
  public bool StartOpen = false;
  public static LoreManager Current;

  public LoreWindowController LoreWindow;

  private LoreWindowOnClose OnClose;
  private bool IsOpen;

  void Start() {
    if (SingletonInstance)
      Current = this;

    SetIsOpen(StartOpen);
  }

  public void Open(LoreItem item, LoreWindowOnClose onClose = null) {
    LoreWindow.ResetZoomAndPan();
    LoreWindow.SetTitle(item.Name);
    LoreWindow.SetImage(item.Image);

    OnClose = onClose;
    SetIsOpen(true);
  }

  public void Close() {
    if (OnClose != null) OnClose();
    SetIsOpen(false);
  }

  void SetIsOpen(bool isOpen) {
    IsOpen = isOpen;

    if (isOpen) {
      LoreWindow.Show();
    } else {
      LoreWindow.Hide();
    }
  }

  void Update() {
    if (IsOpen && WrappedInput.GetButtonUp("Cancel")) {
      Close();
    }
  }
}
