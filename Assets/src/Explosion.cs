using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explosion : MonoBehaviour {
  public Image Image;
  public float Duration;
  public GameObject ExplosionGameObject;

  private Stopwatch Stopwatch;

  void Start() {
    Stopwatch = new Stopwatch.BaseTime();
  }

  void Update() {
    transform.localScale = new Vector3(Scale(), Scale(), Scale());

    var colour = Image.color;
    colour.a = Alpha();
    Image.color = colour;

    if (Progress() >= 1f)
      Destroy(ExplosionGameObject);
  }

  private float Scale() => Mathf.Lerp(0.2f, 1f, Progress());

  private float Alpha() => Mathf.Lerp(1f, 0.75f, Progress());

  private float Progress() => Stopwatch.Progress(Duration);
}
