using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AccessTerminalManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public bool StartOpen = false;
  public static AccessTerminalManager Current;
  public List<GameObject> ActivateOnOpen;
  public UnityEvent OnOpen, OnClose;

  private System.Action InternalOnClose;
  private bool IsOpen;

  void Start() {
    if (SingletonInstance)
      Current = this;

    // For testing
    if (StartOpen)
      Open(new AccessTerminalConfig(), () => { });
  }

  public void Open(AccessTerminalConfig config, System.Action onClose = null) {
    InternalOnClose = onClose;
    SetIsOpen(true);
    OnOpen.Invoke();
  }

  public void Close() {
    if (InternalOnClose != null)
      InternalOnClose();

    OnClose.Invoke();
    SetIsOpen(false);
  }

  private void SetIsOpen(bool isOpen) {
    IsOpen = isOpen;

    foreach (GameObject go in ActivateOnOpen) {
      go.SetActive(isOpen);
    }
  }
}
