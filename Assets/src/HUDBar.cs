using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBar : MonoBehaviour {

  public Image FilledImage;
  public List<Image> AllImages;

  public float Progress;
  public bool Visible;

  void Update() {
    AllImages.ForEach(img => img.enabled = Visible);
    FilledImage.fillAmount = Progress;
  }

}
