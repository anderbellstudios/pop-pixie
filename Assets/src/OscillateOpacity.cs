using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OscillateOpacity : MonoBehaviour {
  public Image Image;

  public float MinSpeed, MaxSpeed;
  private float Offset, Speed;
  private Color InitialColor;

  void Start() {
    Offset = Random.Range(0, Mathf.PI * 2);
    Speed = Random.Range(MinSpeed, MaxSpeed);
    InitialColor = Image.color;
  }

  void Update() {
    float t = (Time.time + Offset) * Speed;
    Image.color = new Color(InitialColor.r, InitialColor.g, InitialColor.b, (Mathf.Sin(t) + 1f) / 2f);
  }
}
