using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ElevatorPanelButton : MonoBehaviour {
  public string Level;
  public int LevelIndex = -1;
  public Color
    AccessibleColor,
    AccessibleSelectedColor,
    InaccessibleColor,
    InaccessibleSelectedColor,
    NextColor,
    NextSelectedColor;
  public Image MetalImage;
  public Oscillate OscillateSize;
  public OscillateBrightness OscillateBrightness;
  public ElevatorEvents ElevatorEvents;
  public UnityEvent OnInaccessible, TemporaryOnNoPreviousLevels;

  private bool Accessible, Next;

  void Start() {
    Accessible = LevelIndex > -1 && LevelIndex <= LevelCompletionData.LevelCompleted + 1;
    Next = LevelIndex > -1 && LevelIndex == LevelCompletionData.LevelCompleted + 1;
    UpdateState();
    SelectionChangeListener.AddListener((current, previous) => UpdateState(), gameObject);
  }

  public void Activate() {
    if (Next) {
      ElevatorEvents.PickLevel(Level);
    } else if (Accessible) {
      TemporaryOnNoPreviousLevels.Invoke();
    } else {
      OnInaccessible.Invoke();
    }
  }

  private void UpdateState() {
    bool selected = EventSystem.current.currentSelectedGameObject == gameObject;

    float metalBrightness = Accessible ? 1f : 0.75f;
    MetalImage.color = new Color(metalBrightness, metalBrightness, metalBrightness, 1f);

    OscillateBrightness.enabled = !selected && Accessible;
    OscillateSize.enabled = selected;

    Color color = OutlineColor(selected);
    OscillateBrightness.SetColor(color);
  }

  private Color OutlineColor(bool selected) {
    if (Next)
      return selected ? NextSelectedColor : NextColor;

    if (Accessible)
      return selected ? AccessibleSelectedColor : AccessibleColor;

    return selected ? InaccessibleSelectedColor : InaccessibleColor;
  }
}
