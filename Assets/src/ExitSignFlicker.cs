using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSignFlicker : MonoBehaviour {

  public SpriteRenderer SpriteRenderer;
  public float Phase = 0;

  void Update() {
    Phase += (Random.value - 0.4f) * Time.deltaTime;
    float intensity = Mathf.Abs(Mathf.Sin(Phase * 5));
    SpriteRenderer.color = new Color(intensity, intensity, intensity, 1f);
  }

}
