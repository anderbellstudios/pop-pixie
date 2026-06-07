using UnityEngine;
using UnityEngine.UI;

public class RhythmGameNoteSprite : MonoBehaviour {
  public Image Arrow;
  public Image Trail;
  public RectTransform ArrowTransform;
  public RectTransform TrailTransform;

  public bool IsHolding { get; private set; } = false;

  private RhythmGameNote Note;

  public void Initialize(RhythmGameNote note, Color color, Quaternion rotation, float trailLength) {
    Note = note;
    Arrow.color = color;
    Trail.color = color;
    TrailTransform.sizeDelta = new Vector2(TrailTransform.sizeDelta.x, trailLength);
    ArrowTransform.localRotation = rotation;
    gameObject.SetActive(true);
  }

  public void StartHolding() {
    Arrow.enabled = false;
    IsHolding = true;
  }

  public void UpdateHolding(float time) {
    float remainingDuration = Note.RelativeTime(time, end: true);
    Trail.fillAmount = remainingDuration / Note.Duration;
  }

  public void StopHolding(float time, bool closeEnough) {
    IsHolding = false;

    if (closeEnough) {
      Trail.enabled = false;
    } else {
      Trail.color = new Color(Trail.color.r, Trail.color.g, Trail.color.b, 0.5f);
    }
  }

  public float ArrowHeight => ArrowTransform.rect.height;
}
