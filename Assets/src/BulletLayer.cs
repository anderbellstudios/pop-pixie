using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLayer : MonoBehaviour {
  public string DefaultLayer = "Character";
  public string TouchingWalkBoundaryLayer = "Top layer";
  public SpriteRenderer SpriteRenderer;
  public TrailRenderer TrailRenderer;

  public int TouchingWalkBoundaries = 0;

  void OnTriggerEnter2D(Collider2D collider) {
    TouchingWalkBoundaries++;
    UpdateLayer();
  }

  void OnTriggerExit2D(Collider2D collider) {
    TouchingWalkBoundaries--;
    UpdateLayer();
  }

  private void UpdateLayer() {
    string currentLayer = TouchingWalkBoundaries > 0
      ? TouchingWalkBoundaryLayer
      : DefaultLayer;
    SpriteRenderer.sortingLayerName = currentLayer;
    TrailRenderer.sortingLayerName = currentLayer;
  }
}
