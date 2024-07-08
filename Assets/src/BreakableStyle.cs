using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStyle : MonoBehaviour {
  public float Speed = 1f;
  public SpriteRenderer SpriteRenderer;

  void Awake() {
    Sprite sprite = SpriteRenderer.sprite;

    Vector4 rect = new Vector4(
      sprite.textureRect.min.x / sprite.texture.width,
      sprite.textureRect.min.y / sprite.texture.height,
      sprite.textureRect.max.x / sprite.texture.width,
      sprite.textureRect.max.y / sprite.texture.height
    );

    SpriteRenderer.material.SetVector("_Rect", rect);
  }

  void Update() {
    SpriteRenderer.material.SetFloat("_OpacityProgress", (Mathf.Sin(Time.time * Speed * Mathf.PI) + 1f) / 2f);
  }

  public void DisableOutline() {
    SpriteRenderer.material.SetFloat("_Enabled", 0f);
  }
}
