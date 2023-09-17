using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingGameMainMenuDateTime : MonoBehaviour {
  public TMP_Text Text;

  void Start() {
    InvokeRepeating("UpdateTime", 0f, 1f);
  }

  void UpdateTime() {
    DateTime dateTime = DateTime.Now;
    Text.text = dateTime.ToString("dddd d MMMM\nHH:mm:ss");
  }
}
