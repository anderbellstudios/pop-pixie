using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TMPMinSize : MonoBehaviour, ILayoutElement {
  public TMP_Text Text;

  public float minWidth { get { return Text.preferredWidth; } }
  public float minHeight { get { return -1; } }
  public float preferredWidth { get { return Text.preferredWidth; } }
  public float preferredHeight { get { return -1; } }
  public float flexibleWidth { get { return -1; } }
  public float flexibleHeight { get { return -1; } }
  public int layoutPriority { get { return 1; } }

  public void CalculateLayoutInputHorizontal() {}
  public void CalculateLayoutInputVertical() {}
}
