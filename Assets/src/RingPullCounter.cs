using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RingPullCounter : MonoBehaviour {

  public TMP_Text Text;

  public Transform PulseTarget;
  public float PulseDuration;
  public float PulseAmplitude;
  public AudioClip PulseSound;
  public SoundController PulseSoundController;

  IntervalTimer PulseTimer;

  void Awake() {
    PulseTimer = new IntervalTimer() {
      Interval = PulseDuration
    };
  }

  void Update() {
    Text.text = RingPullsData.Amount().ToString();

    if ( RingPullsData.ShouldPulse ) {
      RingPullsData.ShouldPulse = false;
      PulseSoundController.Play( PulseSound );
      PulseTimer.Reset();
    }

    if ( PulseTimer.Started ) {
      float scale = 1 + PulseAmplitude * ( 1 - PulseTimer.Progress() );
      PulseTarget.localScale = new Vector2( scale, scale );
    }
  }

}
