using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
  private bool _IsOpen;
  public bool IsOpen;

  public PlaySound PlaySound;
  public BoxCollider2D DoorCollider;
  public GameObject OpenSprite, ClosedSprite;

  void Start() {
    SetIsOpen(IsOpen);
    PlaySound.Play();
  }

  void Update() {
    if (IsOpen != _IsOpen) {
      SetIsOpen(IsOpen);
    }
  }

  public void Open() => SetIsOpen(true);
  public void Close() => SetIsOpen(false);

  private void SetIsOpen(bool isOpen) {
    IsOpen = _IsOpen = isOpen;
    PlaySound.EventInstance.setParameterByName("Is Open", isOpen ? 1f : 0f);
    DoorCollider.enabled = !isOpen;
    OpenSprite.SetActive(isOpen);
    ClosedSprite.SetActive(!isOpen);
    PathfindingGraph.Current?.Recompute();
  }
}
