using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OscillateOpacity : MonoBehaviour {
  public Image Image;

  public float MinSpeed, MaxSpeed;
  private float Offset, Speed;

  void Start() {
    Offset = Random.Range(0, Mathf.PI * 2);
    Speed = Random.Range(MinSpeed, MaxSpeed);
  }

  void Update() {
    float t = (Time.time + Offset) * Speed;
    Image.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(t)));
  }
}
