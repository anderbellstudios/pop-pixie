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

  private Stopwatch Stopwatch = null;
  private Color InitialColor;

  public void Play() {
    InitialColor = SpriteRenderer.color;

    ParticleSystems.ForEach(x => x.Play());
    AsyncTimer.BaseTime.SetTimeout(StartFadeOut, FadeOutDelay);
    AsyncTimer.BaseTime.SetTimeout(DestroyGameObject, DestroyTime);

    if (SpawnFlyingRingPull != null)
      SpawnFlyingRingPull.Instantiate();
  }

  private void StartFadeOut() {
    Stopwatch = new Stopwatch.BaseTime();
  }

  private void DestroyGameObject() {
    Destroy(GameObject);
  }

  void Update() {
    if (Stopwatch == null)
      return;

    SpriteRenderer.color = new Color(
      InitialColor.r,
      InitialColor.g,
      InitialColor.b,
      Mathf.Clamp(1f - Stopwatch.Progress(FadeOutDuration), 0, InitialColor.a)
    );
  }
}
