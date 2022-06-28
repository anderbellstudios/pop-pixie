using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftingBackgroundColor : MonoBehaviour {

  public Camera Camera;
  public float Speed, Saturation, Velocity;

  void Update() {
    float hue = Mathf.Abs(Mathf.Sin(Time.time * Speed));
    Camera.backgroundColor = Color.HSVToRGB(hue, Saturation, Velocity);
  }
}
