using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetTextOpacity : MonoBehaviour {
  public TMP_Text Text;

  public void SetOpacity(float opacity) {
    Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, opacity);
  }
}
