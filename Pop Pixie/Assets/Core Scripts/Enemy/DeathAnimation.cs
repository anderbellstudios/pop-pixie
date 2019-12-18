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

  void Awake() {
    Timer = new IntervalTimer() {
      Interval = FadeOutDuration
    };
  }

  public void Play() {
    ParticleSystems.ForEach( x => x.Play() );
    Invoke("StartFadeOut", FadeOutDelay);
    Invoke("DestroyGameObject", DestroyTime);

    if ( SpawnFlyingRingPull != null )
      SpawnFlyingRingPull.Instantiate();
  }

  void StartFadeOut() {
    Timer.Reset();
  }

  void Update() {
    if ( Timer.Started )
      SpriteRenderer.color = new Color( 0, 0, 0, 1f - Timer.Progress() );
  }

  void DestroyGameObject() {
    Destroy(GameObject);
  }

}
