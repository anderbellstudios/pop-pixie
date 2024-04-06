using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public abstract class ABaseTest {
  protected void SandboxGameData() {
    GameData.FileName = "game-test-" + System.Guid.NewGuid().ToString();
  }

  protected List<GameObject> FindAllByText(string pattern, bool regex = false) {
    return GameObject.FindObjectsOfType<GameObject>().Where(go => {
      TMP_Text textComponent = go.GetComponent<TMP_Text>();
      if (!textComponent)
        return false;
      string text = textComponent.text ?? "";
      return regex
        ? Regex.IsMatch(text, pattern)
        : text == pattern;
    }).ToList();
  }

  protected GameObject FindByText(string text, bool regex = false) {
    List<GameObject> matchingGameObjects = FindAllByText(text, regex);

    if (matchingGameObjects.Count == 0) {
      return null;
    }

    if (matchingGameObjects.Count > 1) {
      Assert.Fail("FindByText: Found more than one GameObject with text: " + text);
    }

    return matchingGameObjects[0];
  }

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

  protected void AssertHasText(string expected, bool regex = false) {
    int matchCount = FindAllByText(expected, regex).Count;
    Assert.AreEqual(1, matchCount, "AssertHasText: Found " + matchCount + " GameObjects with text: " + expected);
  }

  protected void RefuteHasText(string expected, bool regex = false) {
    int matchCount = FindAllByText(expected, regex).Count;
    Assert.AreEqual(0, matchCount, "RefuteHasText: Found " + matchCount + " GameObjects with text: " + expected);
  }

  protected void Click(GameObject go) {
    if (go == null) {
      Assert.Fail("Click: GameObject is null");
    }

    Button button = go.GetComponentInParent<Button>();

    if (button == null) {
      Assert.Fail("Click: GameObject is not inside a Button");
    }

    button.onClick.Invoke();
  }

  protected void ClickByText(string text, bool regex = false) {
    Click(FindByText(text, regex));
  }

  protected string GetActiveScene() {
    return SceneManager.GetActiveScene().name;
  }

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

  protected IEnumerator AwaitSceneChange(string sceneName) {
    yield return AwaitCondition(
      condition: () => GetActiveScene() == sceneName,
      message: "AwaitSceneChange: Timed out waiting for scene change to " + sceneName
    );
  }

  protected IEnumerator AwaitText(string text, bool regex = false, float retryInterval = 1f, int retries = 10) {
    yield return AwaitCondition(
      condition: () => !!FindByText(text, regex),
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

  protected GameObject Player() {
    GameObject player = PlayerGameObject.Current;

    if (player == null) {
      Assert.Fail("Player GameObject does not exist");
    }

    return player;
  }

  protected IEnumerator ScriptedMovement(string[] anchorNames) {
    GameObject player = Player();

    GameObject anchorContainer = GameObject.Find("ScriptedMovementAnchors");

    if (anchorContainer == null) {
      Assert.Fail("ScriptedMovement: ScriptedMovementAnchors not found");
    }

    bool finished = false;

    player.GetComponent<ScriptedMovement>().FollowPath(
      path: anchorNames.Select(anchorName => {
        Transform anchor = anchorContainer.transform.Find(anchorName);

        if (anchor == null) {
          Assert.Fail("ScriptedMovement: Anchor not found: " + anchorName);
        }

        return anchor.position;
      }).ToList(),
      speed: 10f,
      onComplete: () => {
        finished = true;
      }
    );

    yield return AwaitCondition(condition: () => finished, retryInterval: 5f);
  }

  protected IEnumerator ScriptedMovement(string anchorName) {
    yield return ScriptedMovement(new[] { anchorName });
  }

  protected void KillAllEnemies() {
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    if (enemies.Count() == 0) {
      Assert.Fail("KillAllEnemies: No enemies found");
    }

    foreach (GameObject enemy in enemies) {
      HitPoints hp = enemy.GetComponent<HitPoints>();
      if (hp)
        hp.Damage(1000000);
    }
  }

  protected IEnumerator PressButton(string rawButtonName, float duration = 0.2f) {
    string buttonName = rawButtonName.ToLower();

    WrappedInput.TestMode = true;

    WrappedInput.GetButtonOverrides[buttonName] = true;
    WrappedInput.GetButtonDownOverrides[buttonName] = true;

    yield return null;

    WrappedInput.GetButtonDownOverrides[buttonName] = false;

    yield return new WaitForSeconds(duration);

    WrappedInput.GetButtonOverrides[buttonName] = false;
    WrappedInput.GetButtonUpOverrides[buttonName] = true;

    yield return null;

    WrappedInput.GetButtonUpOverrides[buttonName] = false;

    WrappedInput.TestMode = false;
  }

  protected IEnumerator AdvanceDialogue() {
    yield return PressButton("Confirm");
  }
}
