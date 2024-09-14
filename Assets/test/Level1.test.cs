#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class Level1Test : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene("Level 1");
    yield return null;
    yield return AwaitPlayingState();
  }

  [UnityTest]
  public IEnumerator CompletesLevel() {
    // Wait for camera to settle
    yield return new WaitForSeconds(1f);
    yield return SnapPlayer(0.1f);
    yield return TakePercyScreenshot("Level1.1");

    yield return ScriptedMovement(new[] {
      "Middle",
      "Top",
      "Elevator"
    });

    yield return TakePercyScreenshot("Level1.2");

    AssertHasText("Use an.*Access Terminal.*to", regex: true);

    yield return ScriptedMovement(new[] { "Intel", "Terminal" });

    AssertHasText("Find a.*Keycard.*to", regex: true);

    yield return KillAllEnemiesAndAwaitKeycard();

    yield return ScriptedMovement(new[] { "Intel", "Elevator" });

    AssertHasText("Use an.*Access Terminal.*to", regex: true);

    yield return ScriptedMovement(new[] { "Intel", "Terminal" });

    AssertHasText("Press.*to use", regex: true);

    yield return PressButton("Inspect");

    yield return new WaitForSeconds(1f);
    yield return TakePercyScreenshot("Level1.AccessTerminal");

    yield return AwaitText("Mentoes Tower brochure", retries: 60);
    yield return TakePercyScreenshot("Level1.MentoesBrochure");

    yield return PressButton("Cancel");
    yield return AwaitPlayingState();

    AssertHasText("1.*undiscovered.*Piece of Intel", regex: true);

    yield return ScriptedMovement("Intel");

    AssertHasText("Press.*to steal", regex: true);

    yield return PressButton("Inspect");

    AssertHasText("Presence board");
    yield return TakePercyScreenshot("Level1.PresenceBoard");

    yield return TestLoreWindowControls();

    yield return PressButton("Cancel");

    RefuteHasText("1.*undiscovered.*Piece of Intel", regex: true);

    yield return ScriptedMovement("Elevator");

    AssertHasText("Press.*to use", regex: true);

    yield return PressButton("Inspect");
    yield return PressButton("Confirm");

    yield return AwaitSceneChange("Elevator");
  }

  private IEnumerator TestLoreWindowControls() {
    AssertLoreWindowPanAndZoom(0f, 0f, 1f, "Initial");

    yield return Zoom(0.5f, 0.5f);
    AssertLoreWindowPanAndZoom(0f, panY: 0f, zoom: 2.1f, "Zoom in with axis");

    yield return Zoom(1f, 0.75f);
    AssertLoreWindowPanAndZoom(0f, 0f, 8f, "Max zoom");

    yield return Zoom(-1f, 0.75f);
    AssertLoreWindowPanAndZoom(0f, 0f, 1f, "Min zoom");

    yield return Move(0.25f, 0f, 0.5f);
    AssertLoreWindowPanAndZoom(-37f, 0f, 1f, "Pan right");

    yield return PressButton("Reload");
    AssertLoreWindowPanAndZoom(0f, 0f, 1f, "Reset");

    yield return Zoom(1f, 0.75f);
    yield return Move(0.25f, 0f, 0.5f);
    AssertLoreWindowPanAndZoom(-37f, 0f, 8f, "Pan right while zoomed in");

    yield return PressButton("Reload");
    yield return Drag(
      0.25f, 0.5f,
      0.3f, 0.5f
    );
    AssertLoreWindowPanAndZoom(40f, 0f, 1f, "Pan with mouse");

    yield return PressButton("Reload");
    yield return ClickAt(0.5f, 0.5f);
    AssertLoreWindowPanAndZoom(0f, null, 2f, "Zoom with mouse 1");

    yield return ClickAt(0.5f, 0.5f);
    AssertLoreWindowPanAndZoom(0f, null, 4f, "Zoom with mouse 2");

    yield return ClickAt(0.5f, 0.5f);
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

    float panTolerance = 2f;
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
