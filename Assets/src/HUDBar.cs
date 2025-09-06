using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUDBar : MonoBehaviour {
  public static HUDBar PlayerHitPoints,
    Reload;
  public bool IsPlayerHitPoints,
    IsReload;

  [FormerlySerializedAs("Progress")]
  public float DefaultProgress = 1f;

  [FormerlySerializedAs("Visible")]
  public bool DefaultVisible = true;

  public Image FilledImage;
  public List<Image> AllImages;

  private float Progress;
  private bool Visible;

  void Awake() {
    if (IsPlayerHitPoints)
      PlayerHitPoints = this;

    if (IsReload)
      Reload = this;

    Progress = DefaultProgress;
    Visible = DefaultVisible;
  }

  void Update() {
    AllImages.ForEach(image => image.enabled = Visible);
    FilledImage.fillAmount = Progress;
  }

  public void SetProgress(float progress) {
    Progress = progress;
  }

  public void SetVisible(bool visible) {
    Visible = visible;
  }
}
