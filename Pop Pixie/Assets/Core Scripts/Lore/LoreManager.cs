﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour {

  public delegate void LoreWindowOnClose();

  public static LoreManager Current;

  public LoreWindowController LoreWindow;

  private LoreWindowOnClose OnClose;
  private bool IsOpen;

	void Start() {
    Current = this;

    IsOpen = false;
    LoreWindow.Hide();
	}

  public void Open(LoreItem item, LoreWindowOnClose onClose = null) {
    OnClose = onClose;
    IsOpen = true;

    LoreWindow.SetTitle(item.Name);
    LoreWindow.SetText(item.Text);
    LoreWindow.Show();
  }
	
	void Update() {
    if (!IsOpen)
      return;

    if (WrappedInput.GetButtonUp("Cancel")) {
      LoreWindow.Hide();

      if (OnClose != null)
        OnClose();

      IsOpen = false;
    }
	}

}
