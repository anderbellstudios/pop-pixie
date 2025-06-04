#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using TMPro;

public abstract class ABaseTest {
  protected IEnumerator CommonSetup() {
    string runId = System.Guid.NewGuid().ToString();
    GameData.FileName = "game-test-" + runId;
    ConfigData.FileName = "config-test-" + runId;
    TestMode.Enabled = true;
    GameData.Current.Clear();
    ConfigData.Current.Clear();
    CheckpointData.Reset();
    TestResolution.TestDefault.Apply();
    yield return null;
  }

  [UnityTearDown]
  public IEnumerator CommonTearDown() {
    StopMoving();
    StopZooming();
    ClearMousePosition();
    TestResolution.Default.Apply();
    yield return null;
  }

  protected void LoadSceneNotInBuildSettings(string scenePath)
    => EditorSceneManager.LoadSceneAsyncInPlayMode(
      scenePath,
      new LoadSceneParameters(LoadSceneMode.Single)
    );

  protected List<GameObject> FindAllByText(
    string pattern,
    bool regex = false,
    bool includeInactive = false
  ) => GameObject.FindObjectsOfType<GameObject>(includeInactive).Where(go => {
    TMP_Text textComponent = go.GetComponent<TMP_Text>();
    if (!textComponent)
      return false;
    string text = textComponent.text ?? "";
    return regex
      ? Regex.IsMatch(text, pattern)
      : text == pattern;
  }).ToList();

  protected GameObject FindByText(
    string text,
    bool regex = false,
    bool includeInactive = false
  ) {
    List<GameObject> matchingGameObjects = FindAllByText(text, regex, includeInactive);

    if (matchingGameObjects.Count == 0) {
      return null;
    }

    if (matchingGameObjects.Count > 1) {
      Assert.Fail("FindByText: Found more than one GameObject with text: " + text);
    }

    return matchingGameObjects[0];
  }

  protected Button FindButtonByText(
    string text,
    bool regex = false,
    bool includeInactive = false
  ) => FindByText(text, regex, includeInactive)?.GetComponentInParent<Button>();

  protected void AssertHasText(GameObject go, string expected, bool regex = false) {
    if (go == null) {
      Assert.Fail("AssertHasText: GameObject is null");
    }

    TMP_Text textComponent = go.GetComponent<TMP_Text>();

    if (textComponent == null) {
      Assert.Fail("AssertHasText: GameObject does not have TMP_Text component");
    }

    string actual = textComponent.text ?? "";

    if (regex) {
      bool matches = Regex.IsMatch(actual, expected);
      Assert.IsTrue(matches, "AssertHasText: Text should match pattern: " + expected + ", but was: " + actual);
    } else {
      Assert.AreEqual(expected, actual);
    }
  }

  protected void AssertHasText(
    string expected,
    bool regex = false,
    bool includeInactive = false
  ) {
    int matchCount = FindAllByText(expected, regex, includeInactive).Count;
    Assert.AreEqual(1, matchCount, "AssertHasText: Found " + matchCount + " GameObjects with text: " + expected);
  }

  protected IEnumerator AwaitHasText(
    string expected,
    bool regex = false,
    bool includeInactive = false
  ) {
    yield return AwaitCondition(
      condition: () => {
        int matchCount = FindAllByText(expected, regex, includeInactive).Count;
        if (matchCount > 1)
          Assert.Fail("AwaitHasText: Found " + matchCount + " GameObjects with text: " + expected);
        return matchCount == 1;
      },
      message: "AwaitHasText: Found 0 GameObjects with text: " + expected
    );
  }

  protected void RefuteHasText(
    string expected,
    bool regex = false,
    bool includeInactive = false
  ) {
    int matchCount = FindAllByText(expected, regex, includeInactive).Count;
    Assert.AreEqual(0, matchCount, "RefuteHasText: Found " + matchCount + " GameObjects with text: " + expected);
  }

  protected void Click(Button button) {
    if (button == null) {
      Assert.Fail("Click: Button is null");
    }
    button.onClick.Invoke();
  }

  protected void Click(GameObject go) {
    if (go == null) {
      Assert.Fail("Click: GameObject is null");
    }

    Button button = go.GetComponentInParent<Button>();

    if (button == null) {
      Assert.Fail("Click: GameObject is not inside a Button");
    }

    Click(button);
  }

  protected void ClickByText(
    string text,
    bool regex = false,
    bool includeInactive = false
  ) => Click(FindByText(text, regex, includeInactive));

  protected IEnumerator Hover(GameObject go) {
    if (go == null) {
      Assert.Fail("Hover: GameObject is null");
    }

    GameObject previous = EventSystem.current.currentSelectedGameObject;
    if (previous != null) {
      yield return Unhover(previous);
    }

    PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
    pointerEvent.pointerEnter = go;
    ExecuteEvents.ExecuteHierarchy(go, pointerEvent, ExecuteEvents.pointerEnterHandler);
    yield return null;
  }

  protected IEnumerator Hover(Button button) {
    if (button == null) {
      Assert.Fail("Hover: Button is null");
    }
    yield return Hover(button.gameObject);
  }

  protected IEnumerator Unhover(GameObject go) {
    PointerEventData pointerEvent = new PointerEventData(EventSystem.current);
    ExecuteEvents.ExecuteHierarchy(go, pointerEvent, ExecuteEvents.pointerExitHandler);
    yield return null;
  }

  protected IEnumerator HoverByText(
    string text,
    bool regex = false,
    bool includeInactive = false
  ) => Hover(FindByText(text, regex, includeInactive));

  protected void AssertSelected(GameObject go)
    => Assert.AreEqual(
        go,
        EventSystem.current.currentSelectedGameObject,
        "AssertSelected: Expected GameObject was not selected"
      );

  protected void AssertSelected(Button button)
    => AssertSelected(button.gameObject);

  protected void RefuteSelected(GameObject go)
    => Assert.AreNotEqual(
        go,
        EventSystem.current.currentSelectedGameObject,
        "RefuteSelected: GameObject was selected"
      );

  protected void RefuteSelected(Button button)
    => RefuteSelected(button.gameObject);

  protected GameObject StepperValueByLabel(string text) {
    GameObject label = FindByText(text);
    if (label == null) {
      Assert.Fail("StepperValueByLabel: No matching label");
    }
    return label.transform.parent.Find("Value").gameObject;
  }

  protected string GetActiveScene()
    => SceneManager.GetActiveScene().name;

  protected IEnumerator AwaitCondition(
    System.Func<bool> condition,
    float retryInterval = 1f,
    int retries = 10,
    string message = "AwaitCondition: Timed out"
  ) {
    for (int i = 0; i < retries; i++) {
      if (condition()) {
        yield break;
      }

      yield return new WaitForSeconds(retryInterval);
    }

    Assert.Fail(message);
  }

  protected IEnumerator AwaitSceneChange(string sceneName, float retryInterval = 1f, int retries = 10) {
    // If we're already in the scene, wait to ensure it's reloaded
    if (GetActiveScene() == sceneName) {
      yield return new WaitForSeconds(1f);
    }

    yield return AwaitCondition(
      condition: () => GetActiveScene() == sceneName,
      message: "AwaitSceneChange: Timed out waiting for scene change to " + sceneName,
      retryInterval: retryInterval,
      retries: retries
    );
  }

  protected IEnumerator AwaitText(
    string text,
    bool regex = false,
    bool includeInactive = false,
    float retryInterval = 1f,
    int retries = 10
  ) {
    yield return AwaitCondition(
      condition: () => !!FindByText(text, regex, includeInactive),
      message: "AwaitText: Timed out waiting for text: " + text,
      retryInterval: retryInterval,
      retries: retries
    );
  }

  protected IEnumerator AwaitPlayingState() {
    yield return AwaitCondition(
      condition: () => StateManager.Playing,
      message: "AwaitText: Timed out waiting for Playing state"
    );
  }

  protected GameObject Player(bool allowNull = false) {
    GameObject player = PlayerGameObject.Current;

    if (player == null && !allowNull) {
      Assert.Fail("Player GameObject does not exist");
    }

    return player;
  }

  protected Vector3 GetAnchorPosition(string anchorName) {
    GameObject anchorContainer = GameObject.Find("ScriptedMovementAnchors");

    if (anchorContainer == null) {
      Assert.Fail("ScriptedMovementAnchors not found");
    }

    Transform anchor = anchorContainer.transform.Find(anchorName);

    if (anchor == null) {
      Assert.Fail("Anchor not found: " + anchorName);
    }

    return anchor.position;
  }

  protected IEnumerator ScriptedMovement(string[] anchorNames) {
    ScriptedMovement scriptedMovement = Player().GetComponent<ScriptedMovement>();
    scriptedMovement.ScriptedMovementState = false;

    bool finished = false;

    scriptedMovement.FollowPath(
      path: anchorNames.Select(GetAnchorPosition).ToList(),
      speed: 20f,
      onComplete: () => {
        scriptedMovement.ScriptedMovementState = true;
        finished = true;
      }
    );

    yield return AwaitCondition(condition: () => finished, retries: 60);
    yield return new WaitForSeconds(0.5f);
  }

  protected IEnumerator ScriptedMovement(string anchorName) {
    yield return ScriptedMovement(new[] { anchorName });
  }

  protected IEnumerator DieAndResume() {
    string sceneName = SceneManager.GetActiveScene().name;
    string scenePath = SceneManager.GetActiveScene().path;

    HitPoints.PlayerHitPoints.Kill();
    yield return AwaitSceneChange("Game Over");

    // Clicking "Try again" may not work if the scene is not in build settings
    GameData.LoadOrReset();
    GameOverData.IsRetry = true;
    LoadSceneNotInBuildSettings(scenePath);

    yield return AwaitSceneChange(sceneName);
  }

  protected void KillAllEnemies(Transform container = null) {
    foreach (GameObject enemy in EnemyUtils.InContainer(container)) {
      HitPoints hp = enemy.GetComponent<HitPoints>();
      hp?.Kill();
    }
  }

  protected void ImmobiliseAllEnemies(Transform container = null) {
    foreach (GameObject enemy in EnemyUtils.InContainer(container)) {
      MovementManager movementManager = enemy.GetComponent<MovementManager>();
      if (movementManager) {
        movementManager.enabled = false;
      }
    }
  }

  protected void DisableEnemyColliders(Transform container = null) {
    foreach (GameObject enemy in EnemyUtils.InContainer(container)) {
      Collider2D collider = enemy.GetComponent<Collider2D>();
      if (collider) {
        collider.enabled = false;
      }
    }
  }

  protected IEnumerator KillAllEnemiesAndAwaitKeycard(Transform container = null) {
    KillAllEnemies(container);
    yield return new WaitForSeconds(1.5f);
    AssertHasText("The enemy dropped a.*Keycard", regex: true);
    yield return AdvanceDialogue();
  }

  protected void PreventEnemyMovement(Transform container = null) {
    foreach (GameObject enemy in EnemyUtils.InContainer(container)) {
      MovementManager movementManager = enemy.GetComponent<MovementManager>();
      if (movementManager)
        movementManager.enabled = false;
    }
  }

  protected IEnumerator ButtonDown(string rawButtonName) {
    string buttonName = rawButtonName.ToLower();

    WrappedInput.GetButtonOverrides[buttonName] = true;
    WrappedInput.GetButtonDownOverrides[buttonName] = true;

    yield return null;

    WrappedInput.GetButtonDownOverrides[buttonName] = null;
  }

  protected IEnumerator ButtonUp(string rawButtonName) {
    string buttonName = rawButtonName.ToLower();

    WrappedInput.GetButtonOverrides[buttonName] = null;
    WrappedInput.GetButtonUpOverrides[buttonName] = true;

    yield return null;

    WrappedInput.GetButtonUpOverrides[buttonName] = null;
  }

  protected IEnumerator PressButton(string buttonName, float duration = 0.2f) {
    yield return ButtonDown(buttonName);
    yield return new WaitForSeconds(duration);
    yield return ButtonUp(buttonName);
    yield return null;
  }

  protected void Move(float x, float y) {
    WrappedInput.AxisOverrides["horizontal"] = x;
    WrappedInput.AxisOverrides["vertical"] = y;
  }

  protected void MoveUp() => Move(0f, 1f);
  protected void MoveDown() => Move(0f, -1f);
  protected void MoveLeft() => Move(-1f, 0f);
  protected void MoveRight() => Move(1f, 0f);
  protected void StopMoving() => Move(0f, 0f);

  protected IEnumerator MoveUpAndWait() => MoveAndWait(0f, 1f);
  protected IEnumerator MoveDownAndWait() => MoveAndWait(0f, -1f);
  protected IEnumerator MoveLeftAndWait() => MoveAndWait(-1f, 0f);
  protected IEnumerator MoveRightAndWait() => MoveAndWait(1f, 0f);

  protected IEnumerator Move(float x, float y, float duration) {
    Move(x, y);
    yield return new WaitForSeconds(duration);
    StopMoving();
  }

  protected IEnumerator MoveAndWait(float x, float y) {
    Move(x, y);
    yield return null;
    StopMoving();
    yield return new WaitForSeconds(0.25f);
  }

  protected void Zoom(float zoomAxis) {
    WrappedInput.AxisOverrides["zoom"] = zoomAxis;
  }

  protected void StopZooming() => Zoom(0f);

  protected IEnumerator Zoom(float zoomAxis, float duration) {
    Zoom(zoomAxis);
    yield return new WaitForSeconds(duration);
    StopZooming();
  }

  protected void SetMousePosition(Vector2 position) {
    WrappedInput.MousePositionOverride = position;
  }

  protected void SetMousePosition(float x, float y)
    => SetMousePosition(new Vector2(x, y));

  protected void SetViewportMousePosition(Vector2 position) {
    SetMousePosition(Camera.main.ViewportToScreenPoint(position));
  }

  protected void SetViewportMousePosition(float x, float y)
    => SetViewportMousePosition(new Vector2(x, y));

  protected void ClearMousePosition() {
    WrappedInput.MousePositionOverride = null;
  }

  protected IEnumerator MoveMouseSlightly() {
    SetMousePosition(WrappedInput.MousePosition + Vector3.right);
    yield return null;
    SetMousePosition(WrappedInput.MousePosition - Vector3.right);
    yield return null;
  }

  protected IEnumerator DragViewport(float x1, float y1, float x2, float y2) {
    SetViewportMousePosition(x1, y1);
    yield return ButtonDown("Click");
    yield return null;
    SetViewportMousePosition(x2, y2);
    yield return null;
    yield return ButtonUp("Click");
  }

  protected IEnumerator ClickAtViewport(float x, float y) {
    SetViewportMousePosition(x, y);
    yield return PressButton("Click");
  }

  protected IEnumerator AdvanceDialogue() {
    yield return AwaitCondition(() => GameObject.Find("Dialogue Box"));

    while (GameObject.Find("Dialogue Box")) {
      yield return PressButton("Confirm");
    }
  }

  protected IEnumerator GoThroughDoor() {
    yield return AwaitText("Press.*to go through", regex: true);
    yield return PressButton("Inspect");
    yield return AwaitPlayingState();
  }

  protected void AssertPlayerPosition(Vector3 position)
    => Assert.AreEqual(position, Player().transform.position);

  protected IEnumerator SnapPlayer(float increment) {
    GameObject player = Player(allowNull: true);
    if (player == null)
      yield break;

    Transform playerTransform = Player().transform;

    playerTransform.position = new Vector3(
      Mathf.Round(playerTransform.position.x / increment) * increment,
      Mathf.Round(playerTransform.position.y / increment) * increment,
      playerTransform.position.z
    );

    // Wait for camera to adjust
    yield return null;
  }

  /**
   * Save a screenshot of the game to the Percy directory.
   *
   * Note that this approach will fail to capture any canvas whose render mode
   * is set to Screen Space - Overlay. Changing any such canvases to Screen
   * Space - Camera will fix this.
   *
   * Problems with other approaches for taking screenshots:
   * - ScreenCapture.CaptureScreenshot fails silently in CI
   * - Texture2D.ReadPixels without setting the active render texture and
   *   calling Camera.Render doesn't have the problem with Screen Space -
   *   Overlay, but requires WaitForEndOfFrame, which isn't supported in CI
   */
  protected IEnumerator TakePercyScreenshot(string name) {
    yield return SnapPlayer(0.1f);

    // Reset scale of selected button
    GameObject selected = EventSystem.current.currentSelectedGameObject;
    Animator animator = selected?.GetComponent<Animator>();
    if (animator != null) {
      animator.enabled = false;
    }
    TMP_Text text = selected?.GetComponentInChildren<TMP_Text>();
    if (text != null) {
      text.transform.localScale = Vector3.one;
    }

    Camera camera = Camera.main;

    // Ensure the camera has a solid background
    Color backgroundColor = camera.backgroundColor;
    backgroundColor.a = 1;
    camera.backgroundColor = backgroundColor;

    foreach (TestResolution resolution in TestResolution.ScreenshotResolutions) {
      resolution.Apply();
      yield return null;
      yield return null;

      RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 16);
      RenderTexture previousTargetTexture = camera.targetTexture;

      camera.targetTexture = screenTexture;
      RenderTexture.active = screenTexture;
      camera.Render();

      Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
      renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

      // Clean up
      camera.targetTexture = previousTargetTexture;
      RenderTexture.active = null;

      string path = String.Format(
        "./Percy/{0}-{1}.png",
        name,
        resolution.ShortName
      );

      byte[] byteArray = renderedTexture.EncodeToPNG();
      System.IO.File.WriteAllBytes(path, byteArray);
    }

    TestResolution.TestDefault.Apply();
    yield return null;

    if (animator != null) {
      animator.enabled = true;
    }
  }
}
#endif
