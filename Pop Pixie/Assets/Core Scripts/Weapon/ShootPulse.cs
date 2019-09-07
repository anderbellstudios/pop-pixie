using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootPulse : MonoBehaviour {

  public Image Image;
  public MonoBehaviour DirectionManager;
  public float PulseDuration;

  float ConeAngle;
  float Scale;

  IntervalTimer PulseTimer;
  Vector3 Direction;
  Vector3 Position;

  void Start() {
    PulseTimer = new IntervalTimer() {
      Interval = PulseDuration
    };
  }

  public void Pulse() {
    // PulseTimer.Reset();

    var dm = (IDirectionManager) DirectionManager;
    Direction = dm.Direction;

    Position = transform.parent.position;
  }

  void Update() {
    ConeAngle = 0;
    Scale = 0;

    PulseTimer.UnlessElapsed( PulseUpdate );

    transform.localScale = new Vector3( Scale, Scale, Scale );
    Image.fillAmount = ConeAngle / 360;

    transform.rotation = Quaternion.Euler(0, 0, ConeAngle / 2) * Quaternion.FromToRotation(Vector3.up, Direction);
  }

  void PulseUpdate() {
    ConeAngle = Mathf.Lerp( 0f, 45f, Progress() );
    Scale = Mathf.Lerp( 1f, 0f, Progress() );
    transform.position = Position;
  }

  float Progress() {
    return Mathf.Clamp(
      PulseTimer.TimeSinceElapsed() / PulseDuration,
      0f,
      1f
    );
  }

}
