using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLayer : MonoBehaviour {
  public string DefaultLayer = "Character";
  public string TouchingWalkBoundaryLayer = "Top layer";
  public SpriteRenderer SpriteRenderer;
  public TrailRenderer TrailRenderer;

  private int WalkBoundaryLayer;
  private int TouchingWalkBoundaries = 0;
  public List<Collider2D> TouchingVerticalFaces { get; private set; } = new();

  void Start() {
    WalkBoundaryLayer = LayerMask.NameToLayer("WalkBoundary");
  }

  void OnTriggerEnter2D(Collider2D collider) {
    if (IsWalkBoundary(collider)) {
      TouchingWalkBoundaries++;
      UpdateLayer();
    }

    if (IsVerticalFace(collider)) {
      TouchingVerticalFaces.Add(collider);
    }
  }

  void OnTriggerExit2D(Collider2D collider) {
    if (IsWalkBoundary(collider)) {
      TouchingWalkBoundaries--;
      UpdateLayer();
    }

    if (IsVerticalFace(collider)) {
      TouchingVerticalFaces.Remove(collider);
    }
  }

  private bool IsWalkBoundary(Collider2D collider) =>
    collider.gameObject.layer == WalkBoundaryLayer;

  private bool IsVerticalFace(Collider2D collider) => collider.tag == "Vertical Face";

  private void UpdateLayer() {
    string currentLayer = TouchingWalkBoundaries > 0 ? TouchingWalkBoundaryLayer : DefaultLayer;
    SpriteRenderer.sortingLayerName = currentLayer;
    TrailRenderer.sortingLayerName = currentLayer;
  }
}
