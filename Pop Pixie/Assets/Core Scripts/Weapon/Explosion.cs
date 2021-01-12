using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explosion : MonoBehaviour {

  public Image Image;
  public float Duration;
  public GameObject ExplosionGameObject;

  IntervalTimer Timer;

  void Start() {
    Timer = new IntervalTimer() {
      Interval = Duration
    };

    Timer.Reset();
  }

  void Update() {
    transform.localScale = new Vector3( Scale(), Scale(), Scale() );

    var colour = Image.color;
    colour.a = Alpha();
    Image.color = colour;

    if ( Timer.Elapsed() )
      Destroy(ExplosionGameObject);
  }

  float Scale() {
    return Mathf.Lerp( 0.2f, 1f, Progress() );
  }

  float Alpha() {
    return Mathf.Lerp( 1f, 0.75f, Progress() );
  }

  float Progress() {
    return Timer.TimeSinceElapsed() / Duration;
  }

}
