#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class LoreWindowTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Lore Window.unity");
    yield return AwaitSceneChange("Test Lore Window");
    AssertLoreWindowPanAndZoom(0f, 0f, 1f, "Initial");
  }

  [UnityTest, Retry(3)]
  public IEnumerator PanWithAxis() {
    yield return Setup();
    yield return Move(0.25f, 0f, 0.5f);
    AssertLoreWindowPanAndZoom(-37f, 0f, 1f, "Pan right");
  }

  [UnityTest, Retry(3)]
  public IEnumerator ZoomWithAxis() {
    yield return Setup();
    yield return Zoom(0.5f, 0.5f);
    AssertLoreWindowPanAndZoom(0f, panY: 0f, zoom: 2.1f, "Zoom in");
    yield return Zoom(1f, 0.75f);
    AssertLoreWindowPanAndZoom(0f, 0f, 8f, "Max zoom");
    yield return Zoom(-1f, 0.75f);
    AssertLoreWindowPanAndZoom(0f, 0f, 1f, "Min zoom");
  }

  [UnityTest, Retry(3)]
  public IEnumerator PanWithAxisWhileZoomedIn() {
    yield return Setup();
    yield return Zoom(1f, 0.75f);
    yield return Move(0.25f, 0f, 0.5f);
    AssertLoreWindowPanAndZoom(-37f, 0f, 8f, "Pan right while zoomed in");
  }

  [UnityTest, Retry(3)]
  public IEnumerator PanWithMouse() {
    yield return Setup();
    yield return DragViewport(
      0.25f, 0.5f,
      0.3f, 0.5f
    );
    AssertLoreWindowPanAndZoom(53f, 0f, 1f, "Pan with mouse");
  }

  [UnityTest, Retry(3)]
  public IEnumerator ZoomWithMouse() {
    yield return Setup();
    yield return ClickAtViewport(0.5f, 0.5f);
    AssertLoreWindowPanAndZoom(0f, null, 2f, "Zoom with mouse 1");
    yield return ClickAtViewport(0.5f, 0.5f);
    AssertLoreWindowPanAndZoom(0f, null, 4f, "Zoom with mouse 2");
    yield return ClickAtViewport(0.5f, 0.5f);
    AssertLoreWindowPanAndZoom(0f, null, 1f, "Zoom with mouse 3");
  }

  private void AssertLoreWindowPanAndZoom(
    Nullable<float> panX,
    Nullable<float> panY,
    Nullable<float> zoom,
    string message
  ) {
    Transform contentTransform = GameObject.Find("Content transform").transform;
    Vector2 actualPan = contentTransform.localPosition;

    float actualPanX = actualPan.x;
    float actualPanY = actualPan.y;
    float actualZoom = contentTransform.localScale.x;

    float panTolerance = 6f;
    float zoomTolerance = 0.1f;

    if (panX != null) {
      Assert.That(actualPanX, Is.EqualTo(panX).Within(panTolerance), message + ": pan X");
    }

    if (panY != null) {
      Assert.That(actualPanY, Is.EqualTo(panY).Within(panTolerance), message + ": pan Y");
    }

    if (zoom != null) {
      Assert.That(actualZoom, Is.EqualTo(zoom).Within(zoomTolerance), message + ": zoom");
    }
  }
}
#endif
