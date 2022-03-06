using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSound : MonoBehaviour {
  public static MenuSound current
    => EventSystem.current.gameObject.GetComponent<MenuSound>();

  public EventSystem EventSystem;
  public SoundHopper SoundHopper;

  private GameObject PreviousSelected;
  private bool FirstSelection = true;
  private float PreventSoundsBefore = 0f;

  void Update() {
    GameObject currentSelected = EventSystem.currentSelectedGameObject;

    if (currentSelected != PreviousSelected) {
      if (currentSelected != null && !FirstSelection && Time.time >= PreventSoundsBefore) {
        Play();
      }

      PreviousSelected = currentSelected;
      FirstSelection = false;
    }
  }

  public void Play() {
    SoundHopper.Hop();
  }

  public void PreventImminentSounds() {
    PreventSoundsBefore = Time.time + 0.100f;
  }
}
