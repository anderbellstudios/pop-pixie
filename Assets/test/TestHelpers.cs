using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public abstract class ABaseTest {
  protected void SandboxGameData() {
    GameData.FileName = "game-test-" + System.Guid.NewGuid().ToString();
  }

  protected List<GameObject> FindAllByText(string text) {
    return GameObject.FindObjectsOfType<GameObject>().Where(go => {
      TMP_Text textComponent = go.GetComponent<TMP_Text>();
      return textComponent && textComponent.text == text;
    }).ToList();
  }

  protected GameObject FindByText(string text) {
    List<GameObject> matchingGameObjects = FindAllByText(text);

    if (matchingGameObjects.Count == 0) {
      return null;
    }

    if (matchingGameObjects.Count > 1) {
      Assert.Fail("FindByText: Found more than one GameObject with text: " + text);
    }

    return matchingGameObjects[0];
  }

  protected void AssertHasText(GameObject go, string expected) {
    if (go == null) {
      Assert.Fail("AssertHasText: GameObject is null");
    }

    TMP_Text textComponent = go.GetComponent<TMP_Text>();

    if (textComponent == null) {
      Assert.Fail("AssertHasText: GameObject does not have TMP_Text component");
    }

    Assert.AreEqual(expected, textComponent.text);
  }

  protected void AssertHasText(string expected) {
    int matchCount = FindAllByText(expected).Count;
    Assert.AreEqual(1, matchCount, "AssertHasText: Found " + matchCount + " GameObjects with text: " + expected);
  }

  protected void RefuteHasText(string expected) {
    int matchCount = FindAllByText(expected).Count;
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

  protected void ClickByText(string text) {
    Click(FindByText(text));
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

  protected IEnumerator AwaitText(string text) {
    yield return AwaitCondition(
      condition: () => !!FindByText(text),
      message: "AwaitText: Timed out waiting for text: " + text
    );
  }
}
