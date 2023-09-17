using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathAnimation : MonoBehaviour {

  public float FadeOutDelay;
  public float FadeOutDuration;
  public SpriteRenderer SpriteRenderer;
  public float DestroyTime;
  public GameObject GameObject;
  public List<ParticleSystem> ParticleSystems;
  public SpawnFlyingRingPull SpawnFlyingRingPull;

  private IntervalTimer Timer;
  private Color InitialColor;

  void Awake() {
    Timer = new IntervalTimer() {
      Interval = FadeOutDuration
    };
  }

  public void Play() {
    InitialColor = SpriteRenderer.color;

    ParticleSystems.ForEach(x => x.Play());
    Invoke("StartFadeOut", FadeOutDelay);
    Invoke("DestroyGameObject", DestroyTime);

    if (SpawnFlyingRingPull != null)
      SpawnFlyingRingPull.Instantiate();
  }

  void StartFadeOut() {
    Timer.Reset();
  }

  void Update() {
    if (Timer.Started) {
      SpriteRenderer.color = new Color(
        InitialColor.r,
        InitialColor.g,
        InitialColor.b,
        Mathf.Clamp(1f - Timer.Progress(), 0, InitialColor.a)
      );
    }
  }

  void DestroyGameObject() {
    Destroy(GameObject);
  }

}
