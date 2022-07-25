// https://stackoverflow.com/a/58834101

using UnityEngine;
using UnityEngine.UI;

public class RenderParticleEffect : MonoBehaviour {
  public Camera ParticleSystemCamera;
  public Vector2Int TargetResolution;
  public RawImage RawImage;

  private RenderTexture RenderTexture;

  void Awake() {
    RenderTexture = new RenderTexture(TargetResolution.x, TargetResolution.y, 32);
    ParticleSystemCamera.targetTexture = RenderTexture;

    RawImage.texture = RenderTexture;
  }
}
